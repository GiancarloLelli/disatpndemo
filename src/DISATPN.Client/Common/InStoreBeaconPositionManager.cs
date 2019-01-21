using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace DISATPN.Client.Common
{
    public class InStoreBeaconPositionManager
    {
        public Dictionary<ulong, StaticBeaconModel> FixedPoints;

        public InStoreBeaconPositionManager()
        {
            FixedPoints = new Dictionary<ulong, StaticBeaconModel>();
            SeedData();
        }
        
        private void SeedData()
        {
            FixedPoints.Add(0, new StaticBeaconModel { X = 106, Y = 566, Tag = "L0", Id = Guid.Parse("BED8218D-8AE9-4EDE-BBD9-32942F1C6348"), BluetoothAddress = 30129746708558 });
            FixedPoints.Add(0, new StaticBeaconModel { X = 106, Y = 232, Tag = "L1", Id = Guid.Parse("77D09A9E-48E7-4B53-9285-A214651903DB"), BluetoothAddress = 30129746708559 });
            FixedPoints.Add(0, new StaticBeaconModel { X = 690, Y = 216, Tag = "RO", Id = Guid.Parse("2DE6F734-D134-45DD-827B-F5E06A4F09E1"), BluetoothAddress = 30129746708560 });
            FixedPoints.Add(0, new StaticBeaconModel { X = 716, Y = 578, Tag = "R1", Id = Guid.Parse("EA6CA285-9B85-4BDD-A608-A09087D04BA7"), BluetoothAddress = 30129746708561 });
            FixedPoints.Add(0, new StaticBeaconModel { X = 414, Y = 603, Tag = "EX", Id = Guid.Parse("Exit"), BluetoothAddress = 30129746708562 });
        }
    }

    public class StaticBeaconModel
    {
        public double X { get; set; }

        public double Y { get; set; }

        public string Tag { get; set; }

        public Guid Id { get; set; }

        public ulong BluetoothAddress { get; set; }
    }
}
