using System;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace DiamondStories
{
    class MD5Checksum
    {
        public static async Task<string> CalculateMD5Async(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)) // true means use IO async operations
                {
                    byte[] buffer = new byte[65536];
                    int bytesRead;
                    do
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, 65536);
                        if (bytesRead > 0)
                        {
                            md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                        }
                    } while (bytesRead > 0);

                    md5.TransformFinalBlock(buffer, 0, 0);
                    return BitConverter.ToString(md5.Hash).Replace("-", "").ToUpperInvariant();
                }
            }
        }
    }
}
