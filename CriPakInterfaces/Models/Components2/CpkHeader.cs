using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class CpkHeader : Header, ICpkHeader
    {
        public CpkHeader()
        {
            DisplayName = "CPK_HDR";
            ValidationName = "CPK ";
            Offset = 0x10;
        }
    }
}
