using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakRepository.Repositories;
using CriPakRepository.Writers;
using FileRepository.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileRepository
{
    public class FileExtractor : ExtractorsRepository<IFiles>, IExtractorsRepository<IFiles>
    {
        private readonly Func<IFiles>[] _extractors; 
        private readonly Func<IFiles>[] _writers;

        public FileExtractor(ExtractorRepository<FileMapper, FileWriter, IFiles> fileRepo)
        {
            _extractors = new Func<IFiles>[]
               {
                    () => ExtractFileMeta(fileRepo)
               }; 
            _writers = new Func<IFiles>[]
                {
                    () => WriteFile(fileRepo)
                };
        }

        //TODO: Figure out best way to read in and decompress files.  Do I need the footers?  What are they?
        public override IFiles Extract(IEnumerable<IDisplayList> headers)
        {
            CreateOutDirectory(OutputDirectory);
            var files = Extract(_extractors, headers);
            Write(_writers, files);

            return null;
        }
    }
}
