using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IExtractorsRepository<TOut>
    {
        string OutputDirectory { get; set; }
        string FileName { get; set; }  
        IFiles Extract(IEnumerable<IDisplayList> headers, IProgress<int> progress);
    }
}
