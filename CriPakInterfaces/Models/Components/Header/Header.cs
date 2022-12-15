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
        public string Name { get; set;  }
        public string Selection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Validation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        ulong IHeader.Offset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }                                          
}
