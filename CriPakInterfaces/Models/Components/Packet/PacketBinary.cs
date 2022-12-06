using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components
{

    //look at the read and get methods and determine exactly what the differences are and create an override. 
    public class PacketBinary : PacketEncryption
    {
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

        public string ReadString(int length)
        {
            var value = Encoding.UTF8.GetString(PacketBytes.Take(length).ToArray()).Replace("\0", "");
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
    }
}
