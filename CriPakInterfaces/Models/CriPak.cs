using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class CriPak : ICriPak
    {
        public CriPak()
        {
            Sections = new List<ISection>();
            ViewList = new List<IFileViewer>();
        }

        public string Name => Path.GetFileName(FilePath);
        public string BasePath => Path.GetDirectoryName(FilePath);
        public Encoding Encoding => Encoding.GetEncoding(65001);
        public string FilePath { get; set; }
        public string OutputDirectory { get; set; }
        public List<ISection> Sections { get;set; }
        public List<IFileViewer> ViewList { get; set; }

    }
}
