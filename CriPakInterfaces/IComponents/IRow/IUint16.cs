﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint16 : IValue<ushort>
    {
        ushort Value { get; }
    }
}
