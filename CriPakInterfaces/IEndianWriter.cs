using CriPakInterfaces.Models.Components;

namespace LibCPK.Interfaces
{
    public interface IEndianWriter
    {
        void Write<T>(T value);
        void Write(CriFile entry);
    }
}
