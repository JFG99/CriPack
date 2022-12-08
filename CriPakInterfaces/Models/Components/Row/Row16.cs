﻿using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row16 : IRowValue, IUint16
    {
        public Row16(ushort value) 
        {
            Value = value;
        }

        [DefaultValue(0xFFFF)]
        public ushort Value { get; set; }
    }
}
