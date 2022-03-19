using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Mappers.Files;
using CriPakRepository.Repositories;
using CriPakRepository.Writers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakRepository.Extractors
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
        public override IFiles Extract(IEnumerable<IEntity> headers, string inFile, string outDir)
        {
            CreateOutDirectory(outDir);
            var files = Extract(_extractors, headers, inFile);
            Write(_writers, files);

            return files;
        }
    }
}
