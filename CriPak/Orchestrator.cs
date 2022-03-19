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
        private readonly IEnumerable<IReaderDetailsRepository<IEntity>> _readers; 
        private readonly IEnumerable<IExtractorsRepository<IFiles>> _extractors;

        public Orchestrator(IEnumerable<IReaderDetailsRepository<IEntity>> readers,
                            IEnumerable<IExtractorsRepository<IFiles>> extractors)
        {
            _readers = readers;
            _extractors = extractors;
        }

        public IEnumerable<DisplayList> Read(string inFile)
        {
            var headers = new List<DisplayList>();
            _readers.ToList().ForEach(x => headers.AddRange(x.Read(inFile).OfType<DisplayList>()));
            return headers;
        }
        public IEnumerable<ITocHeader> Extract(string inFile, string outDir)
        {
            var headers = new List<IEntity>();
            var files = new List<IFiles>();
            _readers.ToList().ForEach(x => headers.AddRange(x.ReadHeaders(inFile)));
            _extractors.ToList().ForEach(x => files.Add(x.Extract(headers, inFile, outDir)));
            var test = files.SelectMany(x => x.FileMeta) ;
            return null;
        }
    }
}
