using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public partial class Packet : IPacket
    {
        public Packet()
        {
            PacketBytes = new List<byte>();
            ReadOffset = 0;
        }
        public IEnumerable<byte> PacketBytes { get; set; }
        public IEnumerable<byte> DecryptedBytes { get; set; }
        public long Size => PacketBytes.Count();
        private int LastStringLength{get;set;}
        public long Offset { get; set; }
        public bool IsEncrypted => CheckEncryption();
        protected int ReadOffset { get; set; }
        
        public override string ToString()
        {
            return string.Join( " ", PacketBytes.ToList().Select(x =>  string.Format("{0:X2}", x)));
        }
        public int GetLastStringLength()
        {
            return LastStringLength;
        }
    }
}
