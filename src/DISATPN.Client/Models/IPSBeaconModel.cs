using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace DISATPN.Client.Models
{
    public class IPSBeaconModel
    {
        public Point BeaconStoreLocation { get; set; }
        public double MeasuredDistance { get; set; }
        public ulong BeaconAddress { get; set; }
    }
}
