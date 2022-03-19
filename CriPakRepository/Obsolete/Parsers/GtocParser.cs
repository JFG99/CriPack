using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using System.IO;
using System.Linq;

namespace CriPakRepository.Parsers
{
    public class GtocParser : ParserRepository
    {
        public override bool Parse(CriPak package)  
        {
            package.Reader.BaseStream.Seek((long)package.GtocOffset, SeekOrigin.Begin);

            if (package.Reader.ReadCString(4) != "GTOC")
            {
                package.Reader.Close();
                return false;
            }

            package.ReadUTFData();

            package.GtocPacket = package.UtfPacket;
            package.HeaderInfo.Add(new CriFile
            {
                FileName = "GTOC_HDR",
                FileOffset = package.GtocOffset,
                FileOffsetType = package.GtocOffset.GetType(),
                CompressedFileSize = package.GtocPacket.Length,
                FileOffsetPos = package.GtocOffsetPos,
                TOCName = "CPK",
                FileType = "HDR",
                IsEncrypted = package.IsUtfEncrypted
            });

            //package.Header.Add(new GtocHeader
            //{
            //    DecryptedPacket = package.DecryptedPacket,
            //    OriginalPacket = package.OriginalPacket,
            //    Offset = package.GtocOffset,
            //    OffsetPosition = package.GtocOffsetPos
            //});

            return true;
        }
    }
}
