
using CriPakInterfaces;
using System.IO;

namespace LibCPK.Interfaces
{
    public interface IEndianWriter : IEndian
    {
        Stream BaseStream { get; }
        bool IsOpen { get; }
        void CopyFrom(Stream input, int bytes);
    }
}
