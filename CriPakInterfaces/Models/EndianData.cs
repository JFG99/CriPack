using CriPakInterfaces;
using LibCPK.Interfaces;
using System.IO;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class EndianData : IEndian
    {
        public EndianData() { }

        public EndianData(bool isLittleEndian) { IsLittleEndian = isLittleEndian; Buffer = new byte[8]; }

        public bool IsLittleEndian { get; set; }

        public byte[] Buffer { get; set; }
    }
}
