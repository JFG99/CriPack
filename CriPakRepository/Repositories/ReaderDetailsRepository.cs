﻿using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriPakRepository.Repositories
{
    public abstract class ReaderDetailsRepository<TOut> : IReaderDetailsRepository<TOut>
        where TOut : IDisplayList, new()
    {    
        public string FileName { get; set; }
        public long CurrentPosition { get; set; }
        public IEnumerable<IRowValue> Meta { get; set; }
        public IEnumerable<Row> MetaNew { get; set; }
        private int IdSetter { get; set; }

        public T GetHeader<TMapper, T>(ReaderDetailRepository<TMapper, T> repository)
            where TMapper : IDetailMapper<T>
            where T : IDisplayList, new()
        {
            var data = repository.Get(FileName, MetaNew?.Where(x => x.Name.StartsWith($"{repository.SelectionName}")));
            if (data != null)
            {
                data.Id = IdSetter++;
                //CurrentPosition = data.PacketLength;
            }
            return data;
        }
       
        public IEnumerable<IDisplayList> Get(Func<IDisplayList>[] details)
        {
            
            return details.Select(x => x.Invoke()).ToList();

        }
        public IEnumerable<IDisplayList> Get(Func<IDisplayList>[] details, IEnumerable<Row> meta)
        {
            MetaNew = meta;   
            return details.Select(x => x.Invoke()).ToList();

        }
        public abstract IEnumerable<IDisplayList> Read();
        public abstract IEnumerable<IDisplayList> MapForDisplay(IEnumerable<IDisplayList> headers);

    }
}
