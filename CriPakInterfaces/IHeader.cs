using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IHeader : IEntity
    {
        long MetaOffsetPosition { get; set; }
        ulong PackageOffsetPosition { get; set; }
    }
}
