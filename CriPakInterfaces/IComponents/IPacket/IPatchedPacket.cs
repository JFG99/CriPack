using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IPatchedPacket 
    {
        string ToEncryptedString();
        IEnumerable<byte> Encrypt();
    }
}
