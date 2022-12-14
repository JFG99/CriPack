using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IPatchList : IDisplayList
    {
        ulong LengthDifference { get; set; }
        bool IsPatched { get; set; }
    }
}
