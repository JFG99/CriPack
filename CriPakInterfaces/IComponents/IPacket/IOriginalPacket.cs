using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IOriginalPacket : IPacket
    {
        string ReadString(int length);
        string ReadStringFrom(int offeset, int length);
        long ReadBytes(int length);
        long ReadBytesFrom(int offset, int length, bool setOffset);
        IEnumerable<byte> GetBytes(int length);
        byte GetByteFrom(int offset);
        IEnumerable<byte> GetBytesFrom(int offset, int length);
        IEnumerable<byte> Decrypt();
        string ToDecryptedString();
    }
}
