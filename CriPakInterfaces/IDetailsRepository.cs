
using System.Collections.Generic;

namespace CriPakInterfaces
{
    public interface IDetailsRepository
    {
        string FileName { get; set; }
        IEnumerable<IFileViewer> MapForViewer(IEnumerable<ISection> sections);
        IEnumerable<ISection> Read();
    }
}
