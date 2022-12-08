using CriPakInterfaces.Models.Components.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class DisplayList : IDisplayList
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public ulong Offset { get; set; }
        public long ArchiveLength { get; set; }
        public int ExtractedLength { get; set; }
        public ItemType Type { get; set; }
        public string Size => !Type.Equals(ItemType.HDR) ? 
                                    (Math.Ceiling(ExtractedLength / 1024.0) >= 1024 ?
                                        $"{string.Format("{0:##,###}", Math.Ceiling(ExtractedLength / 1048576.0))} MB" :
                                        $"{string.Format("{0:##,###}", Math.Ceiling(ExtractedLength / 1024.0))} KB") :
                                    "";
        public float Percentage => !Type.Equals(ItemType.HDR) ? (float)Math.Ceiling(ArchiveLength / (float)ExtractedLength * 100) : 0;

        public string TypeString => Type.Equals(ItemType.HDR) ? "HDR" : "FILE";



        public string ValidationName { get; set ; }
        public string SelectionName { get ; set ; }
        public IPacket Packet { get; set; }
        public long PacketLength { get; set; }
    }
}
