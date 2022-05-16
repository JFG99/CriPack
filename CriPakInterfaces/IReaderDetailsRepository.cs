
using CriPakInterfaces.Models;
using System.Collections.Generic;

namespace CriPakInterfaces
{
    public interface IReaderDetailsRepository<TOut> where TOut : IDisplayList
    {
        public string FileName {get;set;}
        IEnumerable<IDisplayList> Read();
        IEnumerable<IDisplayList> MapForDisplay(IEnumerable<IDisplayList> headers);
    }
}
