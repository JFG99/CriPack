using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class ItocHeader : Header, IItocHeader
    {
        public ItocHeader()
        {
            FileName = "ItOC_HDR";
            SelectionName = "Itoc";
            ValidationName = "ITOC";
        }
    }
}
