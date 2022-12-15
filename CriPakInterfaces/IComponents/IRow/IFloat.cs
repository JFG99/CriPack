using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IFloat : IValue<float>
    {
        float Value { get; }
    }
}
