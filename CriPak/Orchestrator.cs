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

        public void Read(string inFile)
        {
            var headers = new List<IEntity>();
            _readers.ToList().ForEach(x => headers.AddRange(x.Read(inFile)));
        }
    }
}
