using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using System;
using System.IO;

namespace CriPakRepository.Writers
{
    public class Writer<T> : IWriter<T>
    {
        public string OutputDirectory { get; set; }
        public string OutputFile { get; set; }
        public string FileName { get; set; }
        public IProgress<int> Progress { get; set; }

        public virtual void Write(IFiles data)
        {
            
        }        
    }
}
