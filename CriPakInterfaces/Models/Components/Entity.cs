using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Entity : IDisplayList
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ValidationName { get; set; }
        public string SelectionName { get; set; }
        public IPacket Packet { get; set; }
        public long PacketLength { get; set; }
    }
}
