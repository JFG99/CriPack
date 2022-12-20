using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IExtractorMapper<out T>
    {
        T Map(IEnumerable<IFileViewer> headers);
    }
}
