using DISATPN.Client.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace DISATPN.Client.Common
{
    // X-Ref CalculateDistance: https://stackoverflow.com/questions/20416218/understanding-ibeacon-distancing/20434019#20434019
    // X-Ref ComputeCoordinates: https://stackoverflow.com/questions/20332856/triangulate-example-for-ibeacons

    public class IndoorPositioningHelper
    {
        private static volatile object m_sync = new object();

        private const double X = 0.89976;
        private const double Y = 7.7095;
        private const double Z = 0.111;
        private const int TX_POWER = -59;

        private Dictionary<ulong, double> m_distance;
        private Dictionary<ulong, DateTime> m_health;

        public IndoorPositioningHelper()
        {
            m_distance = new Dictionary<ulong, double>();
            m_health = new Dictionary<ulong, DateTime>();
        }

        public void Add(ulong address, double distance)
        {
            lock (m_sync)
            {
                m_distance[address] = distance;
                m_health[address] = DateTime.UtcNow;
            }
        }

        public Point ComputePosition()
        {
            var point = new Point(-1, -1);

            lock (m_sync)
            {
                CleanDeadBeacons();

                if (m_health.Count >= 3)
                {
                    var beacons = new List<IPSBeaconModel>();
                    var lastActiveBeacons = m_health.OrderByDescending(x => x.Value).Take(3);

                    foreach (var beacon in lastActiveBeacons)
                    {
                        var lastMeasure = m_distance[beacon.Key];

                        beacons.Add(new IPSBeaconModel
                        {
                            BeaconAddress = beacon.Key,
                            MeasuredDistance = lastMeasure,
                            BeaconStoreLocation = InStoreBeaconPositionManager.GetPosition(beacon.Key)
                        });
                    }

                    // First 3 available beacons with store relative position
                    var b_one = beacons[0];
                    var b_two = beacons[1];
                    var b_three = beacons[2];

                    point = ComputeCoordinates(b_one.BeaconStoreLocation, b_two.BeaconStoreLocation, b_three.BeaconStoreLocation, 
                                               b_one.MeasuredDistance, b_two.MeasuredDistance, b_three.MeasuredDistance);
                }
            }

            return point;
        }

        public double CalculateDistance(int rssi)
        {
            if (rssi == 0)
            {
                return -1.0;
            }

            var ratio = rssi * 1.0 / TX_POWER;

            if (ratio < 1.0)
            {
                return Math.Pow(ratio, 10);
            }
            else
            {
                var distance = X * Math.Pow(ratio, Y) + Z;
                return distance;
            }
        }

        private Point ComputeCoordinates(Point a, Point b, Point c, double dA, double dB, double dC)
        {
            var W = dA * dA - dB * dB - a.X * a.X - a.Y * a.Y + b.X * b.X + b.Y * b.Y;
            var Z = dB * dB - dC * dC - b.X * b.X - b.Y * b.Y + c.X * c.X + c.Y * c.Y;

            var x = (W * (c.Y - b.Y) - Z * (b.Y - a.Y)) / (2 * ((b.X - a.X) * (c.Y - b.Y) - (c.X - b.X) * (b.Y - a.Y)));
            var y = (W - 2 * x * (b.X - a.X)) / (2 * (b.Y - a.Y));
            var y2 = (Z - 2 * x * (c.X - b.X)) / (2 * (c.Y - b.Y));

            y = (y + y2) / 2;
            return new Point(x, y);
        }

        private void CleanDeadBeacons()
        {
            foreach (var deadItem in m_health)
            {
                var offset = DateTime.UtcNow - deadItem.Value;
                if (offset.Seconds > 2)
                {
                    m_health.Remove(deadItem.Key);
                    m_distance.Remove(deadItem.Key);
                }
            }
        }
    }
}
