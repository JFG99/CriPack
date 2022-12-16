using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Packet : PacketBinary, IPacket
    {
        public Packet()
        {
            PacketBytes = new List<byte>();
            ReadOffset = 0;
        }
        public Packet(IEnumerable<byte> bytes)
        {
            PacketBytes = bytes;
            ReadOffset = 0;
        }
        public long Size => PacketBytes.Count();
        public long Offset { get; set; }
        public bool IsEncrypted => CheckEncryption();
        public int GetLastStringLength()
        {
            return LastStringLength;
        }
        public int GetReadOffset()
        {
            return ReadOffset;
        }
    }
}
