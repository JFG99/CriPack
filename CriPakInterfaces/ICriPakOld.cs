using CriPakInterfaces.Models;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface ICriPakOld
    {
        ushort Align { get; set; }
        string BaseName { get; set; }
        string BasePath { get; set; }
        ulong ContentOffset { get; set; }
        long ContentOffsetPos { get; set; }
        Dictionary<string, object> CpkData { get; set; }
        string CpkName { get; set; }
        byte[] CpkPacket { get; set; }
        List<CriFile> CriFileList { get; set; }
        byte[] DecryptedPacket { get; set; }
        List<PackagedFile> DisplayList { get; }
        Encoding Encoding { get; set; }
        byte[] EncryptedPacket { get; set; }
        ulong EtocOffset { get; set; }
        long EtocOffsetPos { get; set; }
        byte[] EtocPacket { get; set; }
        uint Files { get; set; }
        ulong GtocOffset { get; set; }
        long GtocOffsetPos { get; set; }
        byte[] GtocPacket { get; set; }
        List<IMeta> Header { get; set; }
        List<CriFile> HeaderInfo { get; set; }
        bool IsUtfEncrypted { get; set; }
        ulong ItocOffset { get; set; }
        long ItocOffsetPos { get; set; }
        byte[] ItocPacket { get; set; }
        byte[] OriginalPacket { get; set; }
        IEndianReader Reader { get; set; }
        IEndianReader SubReader { get; set; }
        ulong TocOffset { get; set; }
        long TocOffsetPos { get; set; }
        byte[] TocPacket { get; set; }
        int Unk1 { get; set; }
        UTF Utf { get; set; }
        byte[] UtfPacket { get; set; }
        long UtfSize { get; set; }
    }
}