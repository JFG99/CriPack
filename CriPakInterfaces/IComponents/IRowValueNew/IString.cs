﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponentsNew
{
    public interface IString : IRowValue
    {
        string Value { get; set; }
    }
}