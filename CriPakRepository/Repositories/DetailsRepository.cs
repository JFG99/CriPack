using CriPakInterfaces;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriPakRepository.Repositories
{
    public abstract class DetailsRepository<TOut> : IDetailsRepository
        where TOut : ISection, new()
    {
        public string FileName { get; set; }
        //public long CurrentPosition { get; set; }
        //public IEnumerable<IValue> Meta { get; set; }
        public IEnumerable<Row> MetaNew { get; set; }
        private int IdSetter { get; set; }

        public T GetHeader<TMapper, T>(DetailRepository<TMapper, T> repository)
            where TMapper : IDetailMapper2<T>
            where T : TOut, new()
        {
            //TODO: Move out the Selection Code here. Continue Refactoring Repo to use Sections rather than IDisplayList.
            var repoName = repository.GetType().GenericTypeArguments.Where(x => x.Name.Contains("Mapper")).First().Name;
            var selection = repoName.Remove("Mapper").ToUpper();
            var data = repository.Get(FileName, MetaNew?.Where(x => x.Name.ToUpper().StartsWith($"{selection}")), selection.PadRight(4));
            if (data != null)
            {
                data.Name = selection;
                data.Id = IdSetter++;
            }
            return data;
        }
        
        public IEnumerable<ISection> Get(Func<ISection>[] details)
        {            
            return details.Select(x => x.Invoke()).ToList();
        }

        public IEnumerable<ISection> Get(Func<ISection>[] details, IEnumerable<Row> meta)
        {
            MetaNew = meta;   
            return details.Select(x => x.Invoke()).ToList();
        }

        public abstract IEnumerable<ISection> Read();
        public abstract IEnumerable<ISection> MapForDisplay(IEnumerable<ISection> headers);

    }
}
