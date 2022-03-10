using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IPacket
    {
        IEnumerable<byte> PacketBytes { get; set; }
        long Size { get; }
        bool IsEncrypted { get; }
        string ToString();
    }                              
}
