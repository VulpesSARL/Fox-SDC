using Org.BouncyCastle.Apache.Bzip2;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class Compress
    {
        public static byte[] CompressData(byte[] data)
        {
            using (MemoryStream compressed = new MemoryStream())
            {
                using (CBZip2OutputStream s = new CBZip2OutputStream(compressed))
                {
                    s.Write(data, 0, data.Length);
                    s.Finish();
                    byte[] cdata = new byte[compressed.Length];
                    compressed.Seek(0, SeekOrigin.Begin);
                    compressed.Read(cdata, 0, (int)compressed.Length);
                    return (cdata);
                }
            }
        }

        public static byte[] UncompressData(byte[] data)
        {
            using (CBZip2InputStream s = new CBZip2InputStream(new MemoryStream(data)))
            {
                using (MemoryStream uncompressed = new MemoryStream())
                {
                    int b;
                    while ((b = s.ReadByte()) != -1)
                    {
                        uncompressed.WriteByte((byte)b);
                    }

                    byte[] udata = new byte[uncompressed.Length];
                    uncompressed.Seek(0, SeekOrigin.Begin);
                    uncompressed.Read(udata, 0, (int)uncompressed.Length);
                    return (udata);
                }
            }
        }
    }
}
