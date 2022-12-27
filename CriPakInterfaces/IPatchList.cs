using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IPatchList 
    {
        ulong LengthDifference { get; set; }
        bool IsPatched { get; set; }
    }
}
