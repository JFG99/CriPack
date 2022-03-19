using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components2;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IWriter<out T> 
    {
        string OutputDirectory { get; set; }
        string FileName { get; set; }
        void Write(IFiles data);
    }
}
