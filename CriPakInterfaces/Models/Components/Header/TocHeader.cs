using CriPakInterfaces.IComponents;

namespace CriPakInterfaces.Models.Components
{
    public class TocHeader : Header, ITocHeader
    {
        public TocHeader()
        {
            FileName = "TOC_HDR"; 
            SelectionName = "Toc";
            ValidationName = "TOC "; // Has space at end
        }
    }
}
