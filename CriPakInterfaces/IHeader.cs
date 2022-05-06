using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IHeader : IDisplayList
    {
        long MetaOffsetPosition { get; set; }
        ulong PackageOffsetPosition { get; set; }
    }
}
