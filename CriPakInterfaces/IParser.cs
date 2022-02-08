
namespace CriPakInterfaces
{
    public interface IParser<T> where T : IEndian
    {
        bool Parse(IEndianReader br, ulong startOffset);
    }
}
