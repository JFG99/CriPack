using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCPK.Interfaces
{
    public interface IEndianWriter
    {
        void Write<T>(T value);
        void Write(FileEntry entry);
    }
}
