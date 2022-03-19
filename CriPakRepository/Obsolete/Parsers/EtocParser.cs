using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakRepository.Mappers;
using CriPakRepository.Repositories;
using CriPakRepository.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CriPakRepository.Parsers
{
    public class EtocParser : ParserRepository
    {
        public override bool Parse(CriPak package)//, ulong startOffset)
        {
            package.Reader.BaseStream.Seek((long)package.EtocOffset, SeekOrigin.Begin);
            if (package.Reader.ReadCString(4) != "ETOC")
            {
                package.Reader.Close();
                return false;
            }

            package.ReadUTFData();
            package.EtocPacket = package.UtfPacket;
            package.HeaderInfo.Add(new CriFile
            {
                FileName = "ETOC_HDR",
                FileOffset = package.EtocOffset,
                FileOffsetType = package.EtocOffset.GetType(),
                CompressedFileSize = package.EtocPacket.Length,
                FileOffsetPos = package.EtocOffsetPos,
                TOCName = "CPK",
                FileType = "HDR",
                IsEncrypted = package.IsUtfEncrypted
            }) ;

            if (!package.ReadDataRows())
            {
                return false;
            }
            package.MapEtocData();
            return true;
        }
    }
}
