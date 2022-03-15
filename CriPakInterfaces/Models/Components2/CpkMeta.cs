using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class CpkMeta : Meta, ICpkMeta, IHeader
    {
        public CpkMeta()
        {
            DisplayName = "CPK_HDR";
            SelectionName = "CPK";
            ValidationName = "CPK "; //Has space at end of string(4 bytes)
        }
        public uint Files { get; set; }
        public ushort Align { get; set; }
        public long MetaOffsetPosition { get; set; }
        public ulong PackageOffsetPosition { get; set; }
    }          
}
