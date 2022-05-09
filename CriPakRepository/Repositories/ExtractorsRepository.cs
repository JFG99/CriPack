using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using CriPakInterfaces.Models.Components2;
using System.Linq;
using System.Text;

namespace CriPakRepository.Repositories
{
    public abstract class ExtractorsRepository<TOut> : IExtractorsRepository<TOut>
    {
        private IEnumerable<IDisplayList> Headers { get; set; }
        private IFiles Files { get; set; }
        public string OutputDirectory { get; set; }
        public string FileName { get; set; }

        public T ExtractFileMeta<TMapper, TWriter, T>(ExtractorRepository<TMapper, TWriter, T> repository)
            where TMapper : IExtractorMapper<T>
            where TWriter : IWriter<T>
            where T : IFiles
        {
            return repository.Get(Headers);
        }
        public IFiles Extract(Func<IFiles>[] details, IEnumerable<IDisplayList> headers)
        {
            Headers = headers;
            return details.Select(x => x.Invoke()).ToList().First();
        }

        public IFiles WriteFile<TMapper, TWriter, T>(ExtractorRepository<TMapper, TWriter, T> repository)
           where TMapper : IExtractorMapper<T>
           where TWriter : IWriter<T>
           where T : IFiles
        {
            repository.Write(Files, FileName, OutputDirectory);
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

        public abstract IFiles Extract(IEnumerable<IDisplayList> tocHeader);
    }
}
