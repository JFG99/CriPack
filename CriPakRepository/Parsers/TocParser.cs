using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
using CriPakRepository.Mappers;
using CriPakRepository.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Parsers
{
    public class TocParser : ParserRepository
    {
        public override bool Parse(CriPak package)
        {
            package.Reader.BaseStream.Seek((long)package.TocOffset, SeekOrigin.Begin);
            if (package.Reader.ReadCString(4) != "TOC ")
            {
                package.Reader.Close();
                return false;
            }

            package.ReadUTFData();
            package.TocPacket = package.UtfPacket;
            package.HeaderInfo.Add(new CriFile
            {
                FileName = "TOC_HDR",
                FileOffset = package.TocOffset,
                FileOffsetType = package.TocOffset.GetType(),
                CompressedFileSize = package.TocPacket.Length,
                FileOffsetPos = package.TocOffsetPos,
                TOCName = "CPK",
                FileType = "HDR",
                IsEncrypted = package.IsUtfEncrypted
            });

            //package.Header.Add(new TocHeader
            //{
            //    DecryptedPacket = package.DecryptedPacket,
            //    OriginalPacket = package.OriginalPacket,
            //    Offset = package.TocOffset,
            //    OffsetPosition = package.TocOffsetPos
            //});



            if (!package.ReadDataRows())
            {
                return false;
            }
            package.MapTocData();
            return true;
        }
    }
}
