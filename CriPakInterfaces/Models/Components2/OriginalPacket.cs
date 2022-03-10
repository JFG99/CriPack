using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class OriginalPacket : Packet, IOriginalPacket 
    {
        public string ToDecryptedString()
        {
            return string.Join(" ", Decrypt().ToList().Select(x => string.Format("{0:X2}", x)));
        }
        public IEnumerable<byte> Decrypt() => ProcessBytes();
        public string ReadString(int length)
        {
            var value = Encoding.UTF8.GetString(Decrypt().Skip(ReadOffset).Take(length).ToArray());
            ReadOffset += length;
            return value;
        }

        public string ReadStringFrom(int offset, int length)
        {
            var value = Encoding.UTF8.GetString(Decrypt().Skip(offset).Take(length).ToArray());
            return value;
        }

        public long ReadBytes(int length)
        {
            var value = ByteConverter.MapInt[length](Decrypt().Skip(ReadOffset).Take(length).Reverse().ToArray(), 0);
            ReadOffset += length;
            return value;
        }
        public long ReadBytesFrom(int offset, int length, bool setOffset = false)
        {
            var value = ByteConverter.MapInt[length](Decrypt().Skip(offset).Take(length).Reverse().ToArray(), 0);
            if (setOffset) { ReadOffset = offset + length; }
            return value;
        }

        public IEnumerable<byte> GetBytes(int length)
        {
            var value = Decrypt().Skip(ReadOffset).Take(length).ToArray();
            ReadOffset += length;
            return value;
        }

        public IEnumerable<byte> GetBytesFrom(int offset, int length )
        {
            return Decrypt().Skip(offset).Take(length).ToArray();
        }

        public byte GetByteFrom(int offset)
        {
            return Decrypt().Skip(offset).Take(1).First();
        }
    }
}
