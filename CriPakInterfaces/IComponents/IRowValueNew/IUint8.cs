using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponentsNew
{
    public interface IUint8 : IRowValue
    {
        byte Value { get; set; }
    }
}
