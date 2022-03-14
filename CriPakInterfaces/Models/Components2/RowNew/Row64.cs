﻿using CriPakInterfaces.IComponentsNew;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.ComponentsNew
{ 
    public class Row64 : IUint64
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public ulong Value { get; set; }
    }
}