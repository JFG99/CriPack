using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IByteArray : IValue
    {
        byte[] Value { get; set; }
    }
}
