﻿using DISATPN.Client.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DISATPN.Client
{
    public sealed partial class MainPage : Page
    {
        BluetoothLEAdvertisementWatcher m_watcher;
        IndoorPositioningHelper m_positioning;
        private const ushort m_msId = 0x0006;
        private const string m_payload = "L1";

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            m_positioning = new IndoorPositioningHelper();

            var writer = new DataWriter();
            writer.WriteInt32(m_payload.Length);
            writer.WriteString(m_payload);

            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = m_msId;
            manufacturerData.Data = writer.DetachBuffer();

            m_watcher = new BluetoothLEAdvertisementWatcher();
            m_watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);
            m_watcher.SignalStrengthFilter.InRangeThresholdInDBm = -70;
            m_watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -75;
            m_watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(2000);

            m_watcher.Received += Watcher_Received;
            m_watcher.Stopped += Watcher_Stopped;
            m_watcher.Start();
        }

        private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var timestamp = args.Timestamp;
            var advertisementType = args.AdvertisementType;
            var rssi = args.RawSignalStrengthInDBm;
            var localName = args.Advertisement.LocalName;
            var address = args.BluetoothAddress;
            var manufacturerSections = args.Advertisement.ManufacturerData;
            var distance = m_positioning.CalculateDistance(rssi);

            foreach (var manufacturerData in manufacturerSections)
            {
                var data = new byte[manufacturerData.Data.Length];

                using (var reader = DataReader.FromBuffer(manufacturerData.Data))
                {
                    reader.ReadBytes(data);
                }

                var payLoad = BitConverter.ToString(data);
                var company = manufacturerData.CompanyId.ToString("X");
                Debug.WriteLine($"[WATCHER-EVENT] => Handler: {nameof(Watcher_Received)} - Company: 0x{company} - Data: {payLoad}");
            }
        }

        #region Navigation & Stop Handlers

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            m_watcher.Stop();
            m_watcher.Stopped -= Watcher_Stopped;
            m_watcher.Received -= Watcher_Received;
        }

        private void Watcher_Stopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
            => Debug.WriteLine($"[WATCHER-EVENT] => Handler: {nameof(Watcher_Stopped)} - Data: {args.Error.ToString()}");

        #endregion
    }
}
