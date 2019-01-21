using System;
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
        private const double X = 0.89976;
        private const double Y = 7.7095;
        private const double Z = 0.111;
        private const int TX_POWER = -59;

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

        public Point ComputeCoordinates(Point a, Point b, Point c, float dA, float dB, float dC)
        {
            var W = dA * dA - dB * dB - a.X * a.X - a.Y * a.Y + b.X * b.X + b.Y * b.Y;
            var Z = dB * dB - dC * dC - b.X * b.X - b.Y * b.Y + c.X * c.X + c.Y * c.Y;

            var x = (W * (c.Y - b.Y) - Z * (b.Y - a.Y)) / (2 * ((b.X - a.X) * (c.Y - b.Y) - (c.X - b.X) * (b.Y - a.Y)));
            var y = (W - 2 * x * (b.X - a.X)) / (2 * (b.Y - a.Y));
            var y2 = (Z - 2 * x * (c.X - b.X)) / (2 * (c.Y - b.Y));

            y = (y + y2) / 2;
            return new Point(x, y);
        }
    }
}
