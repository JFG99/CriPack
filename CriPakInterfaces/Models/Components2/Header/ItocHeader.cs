using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class ItocHeader : Header, IItocHeader
    {
        public ItocHeader()
        {
            DisplayName = "ItOC_HDR";
            SelectionName = "Itoc";
            ValidationName = "ITOC";
        }
    }
}
