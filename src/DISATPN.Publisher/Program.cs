using DISATPN.Client.Common;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace DISATPN.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var waiter = new EventWaitHandle(false, EventResetMode.ManualReset);

            var writer = new DataWriter();
            writer.WriteInt32(Constants.PAYLOAD.Length);
            writer.WriteString(Constants.PAYLOAD);

            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = Constants.MS_ID;
            manufacturerData.Data = writer.DetachBuffer();

            var publisher = new BluetoothLEAdvertisementPublisher();
            publisher.Advertisement.ManufacturerData.Add(manufacturerData);
            publisher.StatusChanged += Publisher_StatusChanged;
            publisher.Start();

            waiter.WaitOne();
        }

        private static void Publisher_StatusChanged(BluetoothLEAdvertisementPublisher sender, BluetoothLEAdvertisementPublisherStatusChangedEventArgs args)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Handler: {nameof(Publisher_StatusChanged)}");
            sb.AppendLine($"Status: {args.Status.ToString()}");
            sb.AppendLine($"Error: {args.Error.ToString()}");
            sb.AppendLine($"----------------------------------");
            Console.WriteLine(sb.ToString());
        }
    }
}