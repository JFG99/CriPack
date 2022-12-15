using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IArchive
    {
        IEnumerable<ISection> Sections { get; set; }
    }
}
