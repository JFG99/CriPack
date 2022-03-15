using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Repositories
{
    public abstract class ReaderDetailsRepository<TOut> : IReaderDetailsRepository<TOut>
        where TOut : IEntity, new()
    {
        private string FileName { get; set; }
        public long CurrentPosition { get; set; }
        public IEnumerable<IRowValue> Meta { get; set; }
        public IEnumerable<CriPakInterfaces.Models.ComponentsNew.Row> MetaNew { get; set; }
        private int IdSetter { get; set; }

        public T GetHeader<TMapper, T>(ReaderDetailRepository<TMapper, T> repository)
            where TMapper : IDetailMapper<T>
            where T : IEntity, new()
        {
            var data = repository.Get(FileName, MetaNew?.FirstOrDefault(x => x.Name.Equals($"{repository.SelectionName}Offset")));
            if (data != null)
            {
                data.Id = IdSetter++;
                //CurrentPosition = data.PacketLength;
            }
            return data;
        }
       
        public IEnumerable<IEntity> Get(Func<IEntity>[] details, string inFile)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = inFile;
            }
            return details.Select(x => x.Invoke()).ToList();

        }
        public IEnumerable<IEntity> Get(Func<IEntity>[] details, IEnumerable<CriPakInterfaces.Models.ComponentsNew.Row> meta)
        {
            MetaNew = meta;   
            return details.Select(x => x.Invoke()).ToList();

        }
        public abstract IEnumerable<IEntity> Read(string inFile);
        
    }
}
