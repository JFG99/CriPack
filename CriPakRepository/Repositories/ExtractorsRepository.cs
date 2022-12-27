using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using CriPakInterfaces.Models.Components;
using System.Linq;
using System.Text;

namespace CriPakRepository.Repositories
{
    public abstract class ExtractorsRepository<TOut> : IExtractorsRepository<TOut>
    {
        private IEnumerable<IFileViewer> Headers { get; set; }
        private IFiles Files { get; set; }
        public IProgress<int> Progress { get; set; }    
        public string OutputDirectory { get; set; }
        public string FileName { get; set; }

        public T ExtractFileMeta<TMapper, TWriter, T>(ExtractorRepository<TMapper, TWriter, T> repository)
            where TMapper : IExtractorMapper<T>
            where TWriter : IWriter<T>
            where T : IFiles
        {
            return repository.Get(Headers);
        }
        public IFiles Extract(Func<IFiles>[] details, IEnumerable<IFileViewer> headers)
        {
            Headers = headers;
            return details.Select(x => x.Invoke()).ToList().First();
        }

        public IFiles WriteFile<TMapper, TWriter, T>(ExtractorRepository<TMapper, TWriter, T> repository)
           where TMapper : IExtractorMapper<T>
           where TWriter : IWriter<T>
           where T : IFiles
        {
            repository.Write(Files, Progress, FileName, OutputDirectory);
            return null;
        }        

        public void Write(Func<IFiles>[] details, IFiles files)
        {
            Files = files;
            details.ToList().ForEach(x => x.Invoke());
        }

        protected void CreateOutDirectory(string outDir)
        {
            if (!System.IO.Directory.Exists(outDir))
            {
                System.IO.Directory.CreateDirectory(outDir);
            }
            OutputDirectory = outDir;
        }

        public abstract IFiles Extract(IEnumerable<IFileViewer> tocHeader, IProgress<int> progress);
    }
}
