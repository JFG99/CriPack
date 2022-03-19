using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IFloat : IRowValue
    {
        new float Value { get; set; }
        new Type Type { get; set; }
        float GetValue();
    }
}
