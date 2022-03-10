
using CriPakInterfaces.Models;

namespace CriPakInterfaces
{
    public interface IReaderDetailsRepository<TOut> where TOut : IHeader
    {
        IHeader Read(string inFile);
    }
}
