using CriPakInterfaces;
using CriPakInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakComplete
{
    public class Orchestrator 
    {
        private readonly IEnumerable<IReaderDetailsRepository<IEntity>> _readers;

        public Orchestrator(IEnumerable<IReaderDetailsRepository<IEntity>> readers)
        {
            _readers = readers;
        }

        public IEnumerable<DisplayList> Read(string inFile)
        {
            var headers = new List<DisplayList>();
            _readers.ToList().ForEach(x => headers.AddRange(x.Read(inFile).OfType<DisplayList>()));
            return headers;
        }
    }
}
