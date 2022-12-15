using CriPakInterfaces.IComponents;

namespace CriPakInterfaces.Models.Components
{
    public class CpkMeta : Meta, ICpkMeta, IHeader
    {
        public CpkMeta()
        {
            FileName = "CPK_HDR";
            SelectionName = "CPK";
            ValidationName = "CPK "; //Has space at end of string(4 bytes)
        }
        public uint Files { get; set; }
        public ushort Align { get; set; }
        public long MetaOffsetPosition { get; set; }
        public ulong PackageOffsetPosition { get; set; }
        ulong IHeader.Offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }          
}
