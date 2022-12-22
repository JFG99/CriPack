using System.IO;

namespace CriPakInterfaces
{
    public interface IEndianReader : IEndian
    {
        Stream BaseStream { get; }
        bool IsOpen { get; }
        void CopyStream(Stream output, int bytes);
        void Close();
        byte ReadByte();
        byte[] ReadBytes(int count);
        double ReadDouble();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        float ReadSingle();
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();
    }
}
