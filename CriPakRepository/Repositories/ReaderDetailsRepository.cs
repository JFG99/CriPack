using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Repositories
{
    public abstract class ReaderDetailsRepository<TOut> : IReaderDetailsRepository<TOut>
        where TOut : IHeader, new()
    {
        private string FileName { get; set; }

        public T GetHeader<TMapper, T>(ReaderDetailRepository<TMapper, T> repository)
            where TMapper : IDetailMapper<T>
            where T : IHeader, new()
        {
            return repository.Get(FileName);
        }
       
        public IEnumerable<IHeader> Get(Func<IHeader>[] details, string inFile = "")
        {
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = inFile;
            }
            return details.Select(x => x.Invoke()).ToList();

        }
        public abstract IHeader Read(string inFile);
        
    }
}
