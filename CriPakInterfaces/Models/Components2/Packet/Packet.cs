using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class Packet : PacketBinary, IPacket
    {
        public Packet()
        {
            PacketBytes = new List<byte>();
            ReadOffset = 0;
        }

        public long Size => PacketBytes.Count();
        public long Offset { get; set; }
        public bool IsEncrypted => CheckEncryption();
        public int GetLastStringLength()
        {
            return LastStringLength;
        }
    }
}
