using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.IComponents;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using CriPakRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CriPakRepository.Mappers;
using SectionRepository.Mappers;

namespace SectionRepository
{
    public class SectionReader : DetailsRepository<Section>
    {
        private readonly Func<ISection>[] _initialReaders;
        private readonly Func<ISection>[] _readers;

        public SectionReader(DetailRepository<CpkMapper, Section> cpkRepository,
                             DetailRepository<ContentMapper, Section> contentRepository,
                             DetailRepository<TocMapper, Section> tocRepository,
                             DetailRepository<EtocMapper, Section> etocRepository,
                             DetailRepository<GtocMapper, Section> gtocRepository
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
                //  () => GetHeader(itocRepository), //Not Implemented
                    () => GetHeader(etocRepository)
                };
        }

        public override IEnumerable<ISection> Read()
        {
            //TODO:  Change the view to display directory when needed.
            var initialSection = Get(_initialReaders);
            var sections = Get(_readers, initialSection.Where(x => x.Name.Contains("CPK")).First().HeaderData.Rows).Where(x => x != null).ToList();
            sections.AddRange(initialSection.ToList());
            return sections;
        }

        public override IEnumerable<IFileViewer> MapForViewer(IEnumerable<ISection> sections)
        {
            return sections.MapToViewer().OrderBy(x => x.Offset).ThenBy(x => x.Id);
        }
    }
}
