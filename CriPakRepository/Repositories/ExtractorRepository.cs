using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriPakRepository.Repositories
{
    public class ExtractorRepository<TMapper, TWriter, TOut> : IExtractorRepository
        where TMapper : IExtractorMapper<TOut>
        where TWriter : IWriter<TOut>
        where TOut : IFiles
    {
        private readonly TMapper _mapper;
        private readonly TWriter _writer;

        public long CurrentPosition { get; set; }
        private IEnumerable<byte> Buffer { get; set; }
        public ExtractorRepository(TMapper mapper, TWriter writer)
        {
            _mapper = mapper;
            _writer = writer;
        }
        public TOut Get(IEnumerable<IFileViewer> headers)
        {
            return _mapper.Map(headers);
        }

        public void Write(IFiles files, IProgress<int> progress, string fileName, string outDir)
        {
            _writer.OutputDirectory = outDir;
            _writer.FileName = fileName;
            _writer.Progress = progress;
            var fileBlocks = new List<IFiles>();
            fileBlocks.Add(new Files { FileMeta = files.FileMeta.Where(x => x.IsCompressed) });
            fileBlocks.Add(new Files { FileMeta = files.FileMeta.Where(x => !x.IsCompressed) });
            var jobs = new List<Task>();

            foreach( var block in fileBlocks)
            {
                jobs.Add(new Task(() => _writer.Write(block)));
            }

            if (jobs.Any())
            {
#if DEBUG
                foreach (var job in jobs)
                {
                    job.RunSynchronously();
                }
#else
                jobs.ForEach(x => x.Start());
#endif
                var taskResult = Task.WhenAll(jobs).IsCompleted;
                jobs.Clear();
            }
        }        
    }
}
