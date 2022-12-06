using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Header : Meta, IHeader
    {
        public long MetaOffsetPosition { get; set; }
        public ulong PackageOffsetPosition { get; set; }
    }                                          
}
