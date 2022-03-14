using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IPacket
    {
        IEnumerable<byte> PacketBytes { get; set; }
        IEnumerable<byte> DecryptedBytes { get; set; }
        long Offset { get; set; }
        long Size { get; }
        bool IsEncrypted { get; }
        string ToString();
        int GetLastStringLength();
        string ReadCString(int offsetLocation, Encoding encoding, int MaxLength = 255);
        string ReadString(int length);
        string ReadStringFrom(int offeset, int length);
        long ReadBytes(int length);
        long ReadBytesFrom(int offset, int length, bool setOffset);
        IEnumerable<byte> GetBytes(int length);
        IEnumerable<byte> GetBytesFrom(int offset, int length);
        byte GetByteFrom(int offset);        
        void MakeDecyrpted();
        string ToDecryptedString();
    }
}