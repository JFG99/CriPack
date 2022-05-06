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

        public IEnumerable<IDisplayList> Read(CriPak criPak)
        {
            _readers.ToList().ForEach(x => {
                x.FileName = criPak.FilePath;
                criPak.DisplayList.AddRange(x.Read().OfType<DisplayList>());
            });
            return criPak.DisplayList;
        }
        public IEnumerable<ITocHeader> Extract(string inFile, string outDir)
        {
            var headers = new List<IDisplayList>();
            var files = new List<IFiles>();
            _readers.ToList().ForEach(x => headers.AddRange(x.ReadHeaders()));
            _extractors.ToList().ForEach(x => files.Add(x.Extract(headers, inFile, outDir)));
            var test = files.SelectMany(x => x.FileMeta) ;
            return null;
        }
    }
}
