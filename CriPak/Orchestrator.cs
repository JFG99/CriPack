using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Repositories;
using PatchRepository;
using SectionRepository;
using SectionRepository.Mappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakComplete
{
    public class Orchestrator 
    {
        private readonly IEnumerable<IDetailsRepository> _sections;
        private readonly IEnumerable<IExtractorsRepository<IFiles>> _extractors;

        public Orchestrator(IEnumerable<IExtractorsRepository<IFiles>> extractors)
        {
            _sections = new List<SectionReader>()
            {
                new SectionReader(
                   new DetailRepository<CpkMapper, Section>(new CpkMapper()),
                   new DetailRepository<ContentMapper, Section>(new ContentMapper()),
                   new DetailRepository<TocMapper, Section>(new TocMapper()),
                   new DetailRepository<EtocMapper, Section>(new EtocMapper()),
                   new DetailRepository<GtocMapper, Section>(new GtocMapper())
                )
            };
            _extractors = extractors;
        }
         
        public CriPak Read(CriPak criPak)
        {
            _sections.ToList().ForEach(s =>
            {
                s.FileName = criPak.FilePath;
                criPak.Sections.Clear();
                criPak.ViewList.Clear();
                criPak.Sections.AddRange(s.Read());
                criPak.ViewList.AddRange(s.MapForViewer(criPak.Sections).ToList());
            });            
            return criPak;
        }

        public void Extract(CriPak criPak, IProgress<int> progress)
        {            
            _extractors.ToList().ForEach(x => {
                x.FileName = criPak.FilePath;
                x.OutputDirectory = criPak.OutputDirectory;
                x.Extract(criPak.ViewList, progress); 
            });
        }

        public void Patch(CriPak criPak, string patchDir, string cpkDir, bool isNoCompression)
        {
            var fileList = Directory.EnumerateFiles(patchDir, "*.*", SearchOption.AllDirectories)
                                    .Select(x => new KeyValuePair<string, string>(Path.GetFileName(x), x))
                                    .ToDictionary(x => x.Key, x => x.Value);

            var patcher = new Patcher();
            patcher.Patch(criPak, cpkDir, fileList);
        }
    }
}
