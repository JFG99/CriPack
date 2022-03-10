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
    public class MetaRepository : ReaderDetailsRepository<CpkHeader>, IReaderDetailsRepository<IHeader>
    {
        private readonly Func<IHeader>[] _initialReaders; 
        private readonly Func<IHeader>[] _readers;

        public MetaRepository(ReaderDetailRepository<CpkMapper, CpkHeader> cpkRepository,
                              ReaderDetailRepository<ContentMapper, ContentHeader> contentRepository,
                              ReaderDetailRepository<TocMapper, TocHeader> tocRepository,
                              ReaderDetailRepository<GtocMapper, GtocHeader> gtocRepository,
                              ReaderDetailRepository<ItocMapper, ItocHeader> itocRepository,
                              ReaderDetailRepository<EtocMapper, EtocHeader> etocRepository
        )
        {
            _initialReaders = new Func<IHeader>[]
                {
                    () => GetHeader(cpkRepository)
                };
            _readers = new Func<IHeader>[]
                {
                    () => GetHeader(contentRepository),
                    () => GetHeader(tocRepository),
                    () => GetHeader(gtocRepository),
                    () => GetHeader(itocRepository),
                    () => GetHeader(etocRepository)
                };
        }

        public override IHeader Read(string inFile) 
        {
            var test = Get(_initialReaders, inFile);




            // var content = _header.Rows.OfType<IUint64>().Where(x => x.Name == "ContentOffset").First();




            //    var fileTest = new CriFile(content.Name, content.Value, content.Type, content.Position, "CPK", "CONTENT", false);

            //    GetHeaderOffsets(package);
            //    package.HeaderInfo.Add(new CriFile("CONTENT_OFFSET", package.ContentOffset, typeof(ulong), package.ContentOffsetPos, "CPK", "CONTENT", false));
            //    package.Files = (uint)package.Utf.GetRowValue("Files");
            //    package.Align = (ushort)package.Utf.GetRowValue("Align");

            //    if (package.TocOffset != 0xFFFFFFFFFFFFFFFF)
            //    {
            //        var tocParser = new TocParser();
            //        if (!tocParser.Parse(package))
            //        {
            //            return false;
            //        }
            //    }

            //    if (package.EtocOffset != 0xFFFFFFFFFFFFFFFF)
            //    {
            //        var etocParser = new EtocParser();
            //        if (!etocParser.Parse(package))
            //        {
            //            return false;
            //        }
            //    }

            //    Leaving this commented out as the Shining Resonance CPK does not have ITOC

            //    if (package.ItocOffset != 0xFFFFFFFFFFFFFFFF)
            //    {
            //        package.HeaderInfo.Add(new CriFile("ITOC_HDR", package.ItocOffset, typeof(ulong), package.ItocOffsetPos, "CPK", "HDR", false));
            //        var itocParser = new ItocParser();
            //        if (!itocParser.Parse(package))
            //        {
            //            return false;
            //        }
            //    }

            //    if (package.GtocOffset != 0xFFFFFFFFFFFFFFFF)
            //    {
            //        var gtocParser = new GtocParser();
            //        if (!gtocParser.Parse(package))
            //        {
            //            return false;
            //        }
            //    }
            //    package.CriFileList.AddRange(package.HeaderInfo);
            //    package.Reader.Close();
            return null;
        }

        //private void GetHeaderOffsets(ICriPak package)
        //{
        //    package.TocOffset = (ulong)package.Utf.GetRowValue("TocOffset");
        //    package.TocOffsetPos = package.Utf.GetRowPostion("TocOffset");

        //    package.EtocOffset = (ulong)package.Utf.GetRowValue("EtocOffset");
        //    package.EtocOffsetPos = package.Utf.GetRowPostion("EtocOffset");

        //    package.ItocOffset = (ulong)package.Utf.GetRowValue("ItocOffset");
        //    package.ItocOffsetPos = package.Utf.GetRowPostion("ItocOffset");

        //    package.GtocOffset = (ulong)package.Utf.GetRowValue("GtocOffset");
        //    package.GtocOffsetPos = package.Utf.GetRowPostion("GtocOffset");

        //    package.ContentOffset = (ulong)package.Utf.GetRowValue("ContentOffset");
        //    package.ContentOffsetPos = package.Utf.GetRowPostion("ContentOffset");
        //}
    }
}
