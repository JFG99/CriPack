using CriPakInterfaces.Models.Components;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class CriPak
    {
        public CriPak()
        {
            CriFileList = new List<CriFile>();
        }

        public IEndianReader Reader { get; set; }
        public List<CriFile> CriFileList { get; set; }
        public Dictionary<string, object> CpkData { get; set; }
        public Encoding Encoding { get; set; }
        public UTF Utf { get; set; }
        public string CpkName { get; set; }
        public bool IsUtfEncrypted { get; set; }
        public int Unk1 { get; set; }
        public long UtfSize { get; set; }
        public byte[] UtfPacket { get; set; }
        public byte[] CpkPacket { get; set; }
        public byte[] TocPacket { get; set; }
        public byte[] ItocPacket { get; set; }
        public byte[] EtocPacket { get; set; }
        public byte[] GtocPacket { get; set; }
        public ulong TocOffset { get; set; }
        public ulong EtocOffset { get; set; }
        public ulong ItocOffset { get; set; }
        public ulong GtocOffset { get; set; }
        public ulong ContentOffset { get; set; }
    }
}
