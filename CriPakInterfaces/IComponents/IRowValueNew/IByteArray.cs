using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponentsNew
{
    public interface IByteArray : IRowValue
    {
        byte[] Value { get; set; }
    }
}
