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

namespace CriPakRepository.Parsers
{
    public class CpkParser : ParserRepository
    {
        public override bool Parse(CriPak package) 
        {
            package.Reader = new EndianReader<FileStream, EndianData>(File.OpenRead(package.CpkName), new EndianData(true));
            if (package.Reader.ReadCString(4) != "CPK ")
            {
                package.Reader.Close();
                return false;
            }
            package.ReadUTFData();
            package.CpkPacket = package.UtfPacket;
            package.HeaderInfo.Add(new CriFile
            {
                FileName = "CPK_HDR",
                FileOffsetPos = package.Reader.BaseStream.Position + 0x10,
                CompressedFileSize = package.CpkPacket.Length,
                IsEncrypted = package.IsUtfEncrypted,
                FileType = "CPK"
            });

            

            if (!package.ReadDataRows())
            {
                return false;
            }
            package.Header.Add(new CpkHeader
            {
                Packet = 
                    new OriginalPacket()
                    {
                        PacketBytes = package.OriginalPacket,                    
                    },
                Offset = 0x10
            });

            var mapper = new CpkMapper();
            mapper.Map(package.Header.OfType<ICpkHeader>().First());
            var content = package.Header.OfType<ICpkHeader>().First().Rows.OfType<IUint64>().Where(x => x.Name == "ContentOffset").First();
            var fileTest = new CriFile(content.Name, content.Value, content.Type, content.Position, "CPK", "CONTENT", false);

            GetHeaderOffsets(package);
            package.HeaderInfo.Add(new CriFile("CONTENT_OFFSET", package.ContentOffset, typeof(ulong), package.ContentOffsetPos, "CPK", "CONTENT", false));
            package.Files = (uint)package.Utf.GetRowValue("Files");
            package.Align = (ushort)package.Utf.GetRowValue("Align");

            if (package.TocOffset != 0xFFFFFFFFFFFFFFFF)
            {
                var tocParser = new TocParser();
                if (!tocParser.Parse(package))
                {
                    return false;
                }
            }

            if (package.EtocOffset != 0xFFFFFFFFFFFFFFFF)
            {
                var etocParser = new EtocParser();
                if (!etocParser.Parse(package))
                {
                    return false;
                }
            }

            //Leaving this commented out as the Shining Resonance CPK does not have ITOC

            //if (package.ItocOffset != 0xFFFFFFFFFFFFFFFF)
            //{
            //    package.HeaderInfo.Add(new CriFile("ITOC_HDR", package.ItocOffset, typeof(ulong), package.ItocOffsetPos, "CPK", "HDR", false));
            //    var itocParser = new ItocParser();
            //    if (!itocParser.Parse(package))
            //    {
            //        return false;
            //    }
            //}

            if (package.GtocOffset != 0xFFFFFFFFFFFFFFFF)
            {
                var gtocParser = new GtocParser();
                if (!gtocParser.Parse(package))
                {
                    return false;
                }
            }
            package.CriFileList.AddRange(package.HeaderInfo);
            package.Reader.Close();
            return true;
        }

        private void GetHeaderOffsets(CriPak package)
        {
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
        }
    }
}
