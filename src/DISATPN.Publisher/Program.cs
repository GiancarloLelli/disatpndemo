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
            var publisher = new BluetoothLEAdvertisementPublisher();

            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = 0xFFFE;

            var writer = new DataWriter();
            ushort uuidData = 0x1234;
            writer.WriteUInt16(uuidData);

            manufacturerData.Data = writer.DetachBuffer();
            publisher.Advertisement.ManufacturerData.Add(manufacturerData);

            publisher.StatusChanged += Publisher_StatusChanged;
            publisher.Start();

            waiter.WaitOne();
        }

        private static void Publisher_StatusChanged(BluetoothLEAdvertisementPublisher sender, BluetoothLEAdvertisementPublisherStatusChangedEventArgs args)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Status: {args.Status.ToString()}");
            sb.AppendLine($"Error: {args.Error.ToString()}");
            Debug.WriteLine(sb.ToString());
        }
    }
}
