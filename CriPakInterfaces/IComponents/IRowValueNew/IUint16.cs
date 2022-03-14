using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponentsNew
{
    public interface IUint16 : IRowValue
    {
        ushort Value { get; set; }
    }
}
