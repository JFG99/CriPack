using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IFloat : IRowValue<float>
    {
        float Value { get; set; }
    }
}
