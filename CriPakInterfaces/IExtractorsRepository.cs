using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IExtractorsRepository<TOut>
    {
        IFiles Extract(IEnumerable<IDisplayList> headers, string inFile, string outDir);
    }
}
