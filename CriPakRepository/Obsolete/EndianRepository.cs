using LibCPK.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriPakRepository.Models
{
    public class EndianRepository : IEndianRepository
    {
        public BinaryReader Reader { get; set; }
        public BinaryWriter Writer { get; set; }
    }
}
