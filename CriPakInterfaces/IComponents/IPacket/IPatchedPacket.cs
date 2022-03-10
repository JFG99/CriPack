using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IPatchedPacket : IPacket
    {
        string ToEncryptedString();
        IEnumerable<byte> Encrypt();
    }
}
