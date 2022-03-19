
using CriPakInterfaces.Models;
using System.Collections.Generic;

namespace CriPakInterfaces
{
    public interface IReaderDetailsRepository<TOut> where TOut : IEntity
    {
        IEnumerable<IEntity> Read(string inFile);
        IEnumerable<IEntity> ReadHeaders(string inFile);
    }
}
