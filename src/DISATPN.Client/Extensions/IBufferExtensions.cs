using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace DISATPN.Client.Extensions
{
    public static class IBufferExtensions
    {
        public static byte[] ReadAsByteArray(this IBuffer buffer)
        {
            var data = new byte[buffer.Length];

            using (var reader = DataReader.FromBuffer(buffer))
            {
                reader.ReadBytes(data);
            }

            return data.Reverse().ToArray();
        }
    }
}
