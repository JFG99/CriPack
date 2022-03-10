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
        private readonly IEnumerable<IReaderDetailsRepository<IHeader>> _readers;

        public Orchestrator(IEnumerable<IReaderDetailsRepository<IHeader>> readers)
        {
            _readers = readers;
        }

        public void Read(string inFile)
        {
            var headers = new List<IHeader>();
            _readers.ToList().ForEach(x => headers.Add(x.Read(inFile)));
        }
    }
}
