
using System.Collections.Generic;

namespace CriPakInterfaces
{
    public interface IDetailsRepository
    {
        string FileName { get; set; }
        IEnumerable<ISection> Read();
    }
}
