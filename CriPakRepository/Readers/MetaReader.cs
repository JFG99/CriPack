using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CriPakRepository.Mappers;
using CriPakRepository.Parsers;

namespace CriPakRepository.Readers
{
    public class MetaReader : ReaderDetailsRepository<Entity>, IReaderDetailsRepository<IEntity>
    {
        private readonly Func<IEntity>[] _initialReaders; 
        private readonly Func<IEntity>[] _readers;

        public MetaReader(ReaderDetailRepository<CpkMapper, CpkMeta> cpkRepository,
                          ReaderDetailRepository<ContentMapper, ContentHeader> contentRepository,
                          ReaderDetailRepository<TocMapper, TocHeader> tocRepository,
                          ReaderDetailRepository<EtocMapper, EtocHeader> etocRepository,
                          ReaderDetailRepository<GtocMapper, GtocHeader> gtocRepository
                          //ReaderDetailRepository<ItocMapper, ItocHeader> itocRepository //Not Implemented
                          )
        {
            _initialReaders = new Func<IEntity>[]
                {
                    () => GetHeader(cpkRepository)
                };
            _readers = new Func<IEntity>[]
                {
                    () => GetHeader(contentRepository),
                    () => GetHeader(tocRepository),
                    () => GetHeader(gtocRepository),
                //   () => GetHeader(itocRepository), //Not Implemented
                    () => GetHeader(etocRepository)
                };
        }

        public override IEnumerable<IEntity> Read(string inFile)
        {
            var displayList = new List<DisplayList>();
            var initialHeader = Get(_initialReaders, inFile);
            var headers = Get(_readers, initialHeader.OfType<ICpkMeta>().First().Rows);
            displayList.AddRange(initialHeader.OfType<IHeader>().MapHeaderRowsToDisplay());
            displayList.AddRange(headers.OfType<IHeader>().MapHeaderRowsToDisplay());
            displayList.AddRange(headers.OfType<ITocHeader>().First().MapTocRowsToDisplay());
            return displayList.OrderBy(x => x.PackageOffset).ThenBy(x => x.Id);
        }

        public override IEnumerable<IEntity> ReadHeaders(string inFile)
        {
            var initialHeader = Get(_initialReaders, inFile);
            return Get(_readers, initialHeader.OfType<ICpkMeta>().First().Rows);
        }
    }
}
