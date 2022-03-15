using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class DisplayList : IEntity
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public ulong PackageOffset { get; set; }
        public int Size { get; set; }
        public int ExtractedSize { get; set; }
        public string Type { get; set; }
        public float Percentage { get; set; }
        public string ValidationName { get; set ; }
        public string SelectionName { get ; set ; }
        public IPacket Packet { get; set; }
        public long PacketLength { get; set; }
    }
}
