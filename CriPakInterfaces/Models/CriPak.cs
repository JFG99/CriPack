using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class CriPak : ICriPak
    {
        public CriPak()
        {
            DisplayList = new List<DisplayList>(); 
            Headers = new List<IDisplayList>();
            Sections = new List<ISection>();
        }

        public string Name => Path.GetFileName(FilePath);
        public string BasePath => Path.GetDirectoryName(FilePath);
        public Encoding Encoding => Encoding.GetEncoding(65001);
        public string FilePath { get; set; }
        public string OutputDirectory { get; set; }
        public List<DisplayList> DisplayList { get; set; }
        public List<IDisplayList> Headers { get; set; }
        public List<ISection> Sections { get;set; }

    }
}
