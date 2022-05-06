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
        }

        public string FilePath { get; set; }
        public string Name => Path.GetFileName(FilePath);
        public string BasePath => Path.GetDirectoryName(FilePath);
        public Encoding Encoding { get; set; }
        public List<DisplayList> DisplayList { get; set; }

    }
}
