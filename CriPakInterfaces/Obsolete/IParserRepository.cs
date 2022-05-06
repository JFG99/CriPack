
using CriPakInterfaces.Models;

namespace CriPakInterfaces
{
    public interface IParserRepository //<T> where T : IEndian
    {
        bool Parse(CriPakOld package);
    }
}
