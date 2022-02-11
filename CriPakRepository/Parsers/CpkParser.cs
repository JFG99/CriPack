using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Parsers
{
    public class CpkParser : ParserRepository
    {
        public override bool Parse(CriPak package) //(string path, Encoding encoding = null)
        {
            package.Reader = new EndianReader<FileStream, EndianData>(File.OpenRead(package.CpkName), new EndianData(true));

            if (package.Reader.ReadCString(4) != "CPK ")
            {
                package.Reader.Close();
                return false;
            }

            package.ReadUTFData();

            package.CpkPacket = package.UtfPacket;

            var CpkEntry = new CriFile
            {
                FileName = "CPK_HDR",
                FileOffsetPos = package.Reader.BaseStream.Position + 0x10,
                FileSize = package.CpkPacket.Length,
                IsEncrypted = package.IsUtfEncrypted,
                FileType = "CPK"
            };

            package.CriFileList.Add(CpkEntry);

            if (!package.ReadDataRows())
            {
                return false;
            }

            package.TocOffset = (ulong)package.Utf.GetRowValue("TocOffset");
            package.TocOffsetPos = package.Utf.GetRowPostion("TocOffset");

            package.EtocOffset = (ulong)package.Utf.GetRowValue("EtocOffset");
            package.EtocOffsetPos = package.Utf.GetRowPostion("EtocOffset"); 

            package.ItocOffset = (ulong)package.Utf.GetRowValue("ItocOffset");
            package.ItocOffsetPos = package.Utf.GetRowPostion("ItocOffset");

            package.GtocOffset = (ulong)package.Utf.GetRowValue("GtocOffset");
            package.GtocOffsetPos = package.Utf.GetRowPostion("GtocOffset");

            package.ContentOffset = (ulong)package.Utf.GetRowValue("ContentOffset");
            package.ContentOffsetPos = package.Utf.GetRowPostion("ContentOffset");

            package.CriFileList.Add(new CriFile("CONTENT_OFFSET", package.ContentOffset, typeof(ulong), package.ContentOffsetPos, "CPK", "CONTENT", false));

            package.Files = (uint)package.Utf.GetRowValue("Files");
            package.Align = (ushort)package.Utf.GetRowValue("Align");

            if (package.TocOffset != 0xFFFFFFFFFFFFFFFF)
            {
                package.CriFileList.Add(new CriFile("TOC_HDR", package.TocOffset, typeof(ulong), package.TocOffsetPos, "CPK", "HDR", false));
                var tocParser = new TocParser();
                if (!tocParser.Parse(package))
                {
                    return false;
                }
            }

            if (package.EtocOffset != 0xFFFFFFFFFFFFFFFF)
            {
                package.CriFileList.Add(new CriFile("ETOC_HDR", package.EtocOffset, typeof(ulong), package.EtocOffsetPos, "CPK", "HDR", false));

                var etocParser = new EtocParser();
                if (!etocParser.Parse(package))
                {
                    return false;
                }
            }

            //if (ItocOffset != 0xFFFFFFFFFFFFFFFF)
            //{
            //    //CriFile ITOC_entry = new CriFile { 
            //    //    FileName = "ITOC_HDR",
            //    //    FileOffset = ItocOffset, FileOffsetType = typeof(ulong), FileOffsetPos = ITocOffsetPos,
            //    //    TOCName = "CPK",
            //    //    FileType = "FILE", Encrypted = true,
            //    //};

            //    CriFile entry = new CriFile("ITOC_HDR", ItocOffset, typeof(ulong), ITocOffsetPos, "CPK", "HDR", false);
            //    FileTable.Add(entry);

            //    if (!ReadITOC(br, ItocOffset, ContentOffset, Align))
            //        return false;
            //}

            //if (GtocOffset != 0xFFFFFFFFFFFFFFFF)
            //{
            //    CriFile entry = new CriFile("GTOC_HDR", GtocOffset, typeof(ulong), GTocOffsetPos, "CPK", "HDR", false);
            //    FileTable.Add(entry);

            //    if (!ReadGTOC(br, GtocOffset))
            //        return false;
            //}

            //package.Reader.Close();

            //// at this point, we should have all needed file info

            ////utf = null;
            //files = null;
            return true;

        }
    }
}
