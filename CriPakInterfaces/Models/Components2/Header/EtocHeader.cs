using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class EtocHeader : Header, IEtocHeader
    {
        public EtocHeader()
        {
            DisplayName = "ETOC_HDR";
            SelectionName = "Etoc";
            ValidationName = "ETOC";
        }
    }
}
