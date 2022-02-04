using LibCPK.Interfaces;
using System.IO;
using System.Text;

namespace LibCPK.Models
{
    public class EndianData : IEndian
    {
        //public Endian(Stream input, Encoding encoding, bool isLittleEndian) 
        //{
        //    IsLittleEndian = isLittleEndian;
        //}
        public EndianData() { }

        public EndianData(bool isLittleEndian) { IsLittleEndian = isLittleEndian; Buffer = new byte[8]; }

        public bool IsLittleEndian { get; set; }

        public byte[] Buffer { get; set; }
    }
}
