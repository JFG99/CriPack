using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class GtocHeader : Header, IGtocHeader
    {
        public GtocHeader()
        {
            DisplayName = "GTOC_HDR";
            SelectionName = "Gtoc";
            ValidationName = "GTOC";
        }
    }
}
