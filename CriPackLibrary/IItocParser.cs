
namespace CriPakInterfaces
{
    public interface IItocParser<T> : IParser<T> where T : IEndian
    {
        bool Parse(T test, ulong startOffset, ulong contentOffset, ushort align);
    }
}
