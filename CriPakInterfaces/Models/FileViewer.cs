using CriPakInterfaces.Models.Components.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class FileViewer : IFileViewer
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public long Offset { get; set; }
        public ulong ArchiveLength { get; set; }
        public uint ExtractedLength { get; set; }
        public ItemType Type { get; set; }
        public string Size => !Type.Equals(ItemType.HDR) ?
                                    (Math.Ceiling(ExtractedLength / 1024.0) >= 1024 ?
                                        $"{string.Format("{0:##,###}", Math.Ceiling(ExtractedLength / 1048576.0))} MB" :
                                        $"{string.Format("{0:##,###}", Math.Ceiling(ExtractedLength / 1024.0))} KB") :
                                    "";
        public float Percentage => !Type.Equals(ItemType.HDR) ? (float)Math.Ceiling(ArchiveLength / (float)ExtractedLength * 100) : 0;

        public string TypeString => Type.Equals(ItemType.HDR) ? "HDR" : "FILE";

    }
}
