using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.IComponents;
using CriPakRepository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using CriPakRepository.Mappers;
using MetaRepository.Mappers;

namespace MetaRepository
{
    public class SectionReader : DetailsRepository<ISection>
    {
        private readonly Func<ISection>[] _initialReaders; 
        private readonly Func<ISection>[] _readers;

        public SectionReader(DetailRepository<CpkMapper, CpkMeta> cpkRepository,
                          DetailRepository<ContentMapper, ContentHeader> contentRepository,
                          DetailRepository<TocMapper, TocHeader> tocRepository,
                          DetailRepository<EtocMapper, EtocHeader> etocRepository,
                          DetailRepository<GtocMapper, GtocHeader> gtocRepository
                          //DetailRepository<ItocMapper, ItocHeader> itocRepository //Not Implemented
                          ) 
        {
            _initialReaders = new Func<ISection>[]
                {
                    () => GetHeader(cpkRepository)
                };
            _readers = new Func<ISection>[]
                {
                    () => GetHeader(contentRepository),
                    () => GetHeader(tocRepository),
                    () => GetHeader(gtocRepository),
                //   () => GetHeader(itocRepository), //Not Implemented
                    () => GetHeader(etocRepository)
                };
        }

        public override IEnumerable<ISection> Read()
        {
            var sections = new List<Section>();
            var initialHeader = Get(_initialReaders);
            var headers = Get(_readers, initialHeader.OfType<ICpkMeta>().First().Rows).ToList();
            headers.AddRange(initialHeader.ToList());
            return headers;
        }

        public override IEnumerable<ISection> MapForDisplay(IEnumerable<ISection> headers)
        {
            var displayList = new List<ISection>();
            //displayList.AddRange(headers.OfType<IHeader>().MapHeaderRowsToDisplay());
            //displayList.AddRange(headers.OfType<ITocHeader>().First().MapTocRowsToDisplay());
            return displayList; // displayList.OrderBy(x => x.Offset).ThenBy(x => x.Id);
        }
    }
}
