using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class TocHeader : Header, ITocHeader
    {
        public TocHeader()
        {
            DisplayName = "TOC_HDR"; 
            SelectionName = "Toc";
            ValidationName = "TOC "; // Has space at end
        }
    }
}
