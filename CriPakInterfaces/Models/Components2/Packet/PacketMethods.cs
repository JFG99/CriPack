using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public partial class Packet : IPacket
    {
        private IEnumerable<byte> Decrypt() => ProcessBytes();
        protected IEnumerable<byte> ProcessBytes()
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

        private bool CheckEncryption()
        {
            return !string.Join("", PacketBytes.Take(4).ToList().Select(x => string.Format("{0:X2}", x))).Equals($"40555446"); //@UTF
        }

        private IEnumerable<byte> GetDecryptedSegment(int offset, int length)
        {
            return DecryptedBytes.Skip(offset).Take(length);
        }

        private IEnumerable<byte> GetDecryptedSegment(int length)
        {
            return GetDecryptedSegment(ReadOffset, length);
        }

        public string ReadCString(int offsetLocation, Encoding encoding, int MaxLength = 255)
        {
            int i = 0;            
            var bytes = GetDecryptedSegment(offsetLocation, MaxLength).ToArray();
            while (bytes.ToArray()[i] != 0 && i < MaxLength)
            {
                i += 1;
            }
            LastStringLength = i;
            return encoding.GetString(bytes.Take(i).ToArray());
        }

        public string ToDecryptedString()
        {
            return string.Join(" ", DecryptedBytes.ToList().Select(x => string.Format("{0:X2}", x))); 
        }
        
        public string ReadString(int length)
        {
            var value = Encoding.UTF8.GetString(PacketBytes.Take(length).ToArray()).Replace("\0", "");
            ReadOffset += length;
            return value;
        }
        public string ReadEncryptedString(int length)
        {
            var value = Encoding.UTF8.GetString(GetDecryptedSegment(length).ToArray()).Replace("\0", "");
            ReadOffset += length;
            return value;
        }

        public string ReadStringFrom(int offset, int length)
        {
            var value = Encoding.UTF8.GetString(GetDecryptedSegment(offset, length).ToArray()).Replace("\0", "");
            return value;
        }

        public long ReadBytes(int length)
        {
            var value = ByteConverter.MapInt[length](GetDecryptedSegment(length).Reverse().ToArray(), 0);
            ReadOffset += length;
            return value;
        }

        public long ReadBytesFrom(int offset, int length, bool setOffset = false)
        {
            var value = ByteConverter.MapInt[length](GetDecryptedSegment(offset, length).Reverse().ToArray(), 0);
            if (setOffset) { ReadOffset = offset + length; }
            return value;
        }

        public IEnumerable<byte> GetBytes(int length)
        {
            var value = GetDecryptedSegment(length).ToArray();
            ReadOffset += length;
            return value;
        }

        public IEnumerable<byte> GetBytesFrom(int offset, int length)
        {
            return GetDecryptedSegment(offset, length).ToArray();
        }

        public byte GetByteFrom(int offset)
        {
            return GetDecryptedSegment(offset, 1).First();
        }                

        public void MakeDecyrpted()
        {
            DecryptedBytes = Decrypt();
        }
    }
}
