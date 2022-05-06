using CriPakInterfaces.Models.Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class CriPakOld : ICriPakOld
    {
        public CriPakOld()
        {
            CriFileList = new List<CriFile>();
            HeaderInfo = new List<CriFile>();
            Header = new List<IMeta>();
            Utf = new UTF();
            CpkData = new Dictionary<string, object>();
        }
        public List<PackagedFile> DisplayList => CriFileList.OfType<PackagedFile>().Where(x => x.FileType != null).OrderBy(x => x.FileOffset).ThenBy(x => x.FileId).ToList();
        public IEndianReader Reader { get; set; }
        public IEndianReader SubReader { get; set; }
        public List<CriFile> CriFileList { get; set; }
        public List<CriFile> HeaderInfo { get; set; }
        public List<IMeta> Header { get; set; }
        public Dictionary<string, object> CpkData { get; set; }
        public Encoding Encoding { get; set; }
        public UTF Utf { get; set; }
        public uint Files { get; set; }
        public ushort Align { get; set; }
        public string BaseName { get; set; }
        public string BasePath { get; set; }
        public string CpkName { get; set; }
        public bool IsUtfEncrypted { get; set; }
        public int Unk1 { get; set; }
        public long UtfSize { get; set; }
        public byte[] UtfPacket { get; set; }
        //public IEnumerable<IPacket> Packets { get; set; }
        public byte[] OriginalPacket { get; set; }
        public byte[] EncryptedPacket { get; set; }
        public byte[] DecryptedPacket { get; set; }
        public byte[] CpkPacket { get; set; }
        //TOC
        public byte[] TocPacket { get; set; }
        public ulong TocOffset { get; set; }
        public long TocOffsetPos { get; set; }
        //ETOC
        public byte[] EtocPacket { get; set; }
        public ulong EtocOffset { get; set; }
        public long EtocOffsetPos { get; set; }
        //ITOC
        public byte[] ItocPacket { get; set; }
        public ulong ItocOffset { get; set; }
        public long ItocOffsetPos { get; set; }
        //GTOC
        public byte[] GtocPacket { get; set; }
        public ulong GtocOffset { get; set; }
        public long GtocOffsetPos { get; set; }

        public ulong ContentOffset { get; set; }
        public long ContentOffsetPos { get; set; }
    }
}
