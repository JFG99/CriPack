using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CriPakRepository.Mappers;
using MetaRepository.Mappers;

namespace MetaRepository
{
    public class MetaReader : ReaderDetailsRepository<DisplayList>, IReaderDetailsRepository<IDisplayList>
    {
        private readonly Func<IDisplayList>[] _initialReaders; 
        private readonly Func<IDisplayList>[] _readers;

        public MetaReader(ReaderDetailRepository<CpkMapper, CpkMeta> cpkRepository,
                          ReaderDetailRepository<ContentMapper, ContentHeader> contentRepository,
                          ReaderDetailRepository<TocMapper, TocHeader> tocRepository,
                          ReaderDetailRepository<EtocMapper, EtocHeader> etocRepository,
                          ReaderDetailRepository<GtocMapper, GtocHeader> gtocRepository
                          //ReaderDetailRepository<ItocMapper, ItocHeader> itocRepository //Not Implemented
                          ) 
        {
            _initialReaders = new Func<IDisplayList>[]
                {
                    () => GetHeader(cpkRepository)
                };
            _readers = new Func<IDisplayList>[]
                {
                    () => GetHeader(contentRepository),
                    () => GetHeader(tocRepository),
                    () => GetHeader(gtocRepository),
                //   () => GetHeader(itocRepository), //Not Implemented
                    () => GetHeader(etocRepository)
                };
        }

        public override IEnumerable<IDisplayList> Read()
        {
            var displayList = new List<DisplayList>();
            var initialHeader = Get(_initialReaders);
            var headers = Get(_readers, initialHeader.OfType<ICpkMeta>().First().Rows).ToList();
            headers.AddRange(initialHeader.ToList());
            return headers;

        }

        public override IEnumerable<IDisplayList> MapForDisplay(IEnumerable<IDisplayList> headers)
        {
            var displayList = new List<DisplayList>();
            displayList.AddRange(headers.OfType<IHeader>().MapHeaderRowsToDisplay());
            displayList.AddRange(headers.OfType<ITocHeader>().First().MapTocRowsToDisplay());
            return displayList.OrderBy(x => x.PackageOffset).ThenBy(x => x.Id);
        }
    }
}
