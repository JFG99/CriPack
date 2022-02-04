using System.Text;

namespace CriPakInterfaces
{
    public interface ITocParser<T> : IParser<T> where T : IEndian
    {
        bool Parse(T test, ulong startOffset, ulong contentOffset, Encoding encoding);
    }
}