using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakComplete
{
    public class Orchestrator 
    {
        private readonly IEnumerable<IReaderDetailsRepository<IDisplayList>> _readers; 
        private readonly IEnumerable<IExtractorsRepository<IFiles>> _extractors;

        public Orchestrator(IEnumerable<IReaderDetailsRepository<IDisplayList>> readers,
                            IEnumerable<IExtractorsRepository<IFiles>> extractors)
        {
            _readers = readers;
            _extractors = extractors;
        }

        public CriPak Read(CriPak criPak)
        {
            _readers.ToList().ForEach(x => {
                x.FileName = criPak.FilePath;
                criPak.Headers.AddRange(x.Read());
                criPak.DisplayList.AddRange(x.MapForDisplay(criPak.Headers).OfType<DisplayList>().ToList());
            }); 
            
            return criPak;
        }

        public IEnumerable<ITocHeader> Extract(CriPak criPak)
        {
            var files = new List<IFiles>();
            _extractors.ToList().ForEach(x => {
                x.FileName = criPak.FilePath;
                x.OutputDirectory = criPak.OutputDirectory;
                files.Add(x.Extract(criPak.Headers)); 
            });
            var test = files.SelectMany(x => x.FileMeta);
            return null;
        }
    }
}
