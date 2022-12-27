using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class EtocHeader : Header, IEtocHeader
    {
        public EtocHeader()
        {
            FileName = "ETOC_HDR";
            SelectionName = "Etoc";
            ValidationName = "ETOC";
        }
    }
}
