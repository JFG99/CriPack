using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface ICriPak
    {
        string Name { get; }
        string BasePath { get; }
        string OutputDirectory { get; set; }
        string FilePath { get; set; }
        Encoding Encoding { get; } 
    }
}