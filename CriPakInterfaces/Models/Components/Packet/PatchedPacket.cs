using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class PatchedPacket : Packet, IPatchedPacket
    {
        public string ToEncryptedString()
        {
            return string.Join(" ", Encrypt().ToList().Select(x => string.Format("{0:X2}", x)));
        }
        public IEnumerable<byte> Encrypt() => PacketBytes = ProcessBytes(DecryptedBytes);
    }
   
}
