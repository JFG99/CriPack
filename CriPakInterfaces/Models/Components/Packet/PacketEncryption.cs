using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class PacketEncryption : PacketBase
    {
        public void MakeDecrypted()
        {
            DecryptedBytes = Decrypt();
        }
        public string ReadEncryptedString(int length)
        {
            var value = Encoding.UTF8.GetString(GetDecryptedSegment(length).ToArray()).Replace("\0", "");
            ReadOffset += length;
            return value;
        }

        public string ToDecryptedString()
        {
            return string.Join(" ", DecryptedBytes.ToList().Select(x => string.Format("{0:X2}", x)));
        }

        public IEnumerable<byte> ProcessBytes()
        {
            if (CheckEncryption())
            {
                var seed = 0x0000655f;
                var decrypted = new List<byte>();
                foreach (var entry in PacketBytes)
                {
                    decrypted.Add((byte)(entry ^ (byte)(seed & 0xff)));
                    //seed modifier
                    seed *= 0x00004115;
                }
                return decrypted;
            }
            return PacketBytes;
        }

        public bool CheckEncryption()
        {
            return !string.Join("", PacketBytes.Take(4).ToList().Select(x => string.Format("{0:X2}", x))).Equals($"40555446"); //@UTF
        }

        public IEnumerable<byte> Decrypt() => ProcessBytes();

        public IEnumerable<byte> GetDecryptedSegment(int offset, int length)
        {
            if(DecryptedBytes is null)
            {
                MakeDecrypted();
            }
            return DecryptedBytes.Skip(offset).Take(length);
        }

        public IEnumerable<byte> GetDecryptedSegment(int length)
        {
            return GetDecryptedSegment(ReadOffset, length);
        }
    }
}