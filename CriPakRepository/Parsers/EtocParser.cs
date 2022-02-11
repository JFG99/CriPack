using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakRepository.Helpers;
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

            if (!package.ReadDataRows())
            {
                return false;
            }

            //Move to a Mapper
            var updateRowList = package.Utf.Rows.Where(x => x.Name == "UpdateDateTime");
            var localDirList = package.Utf.Rows.Where(x => x.Name == "LocalDir");
            if (updateRowList.Any())
            {
                package.CriFileList = package.CriFileList.Join(updateRowList, t => t.FileId, ur => ur.Id, (t, ur) =>
                {
                    t.UpdateDateTime = ur.uint64;
                    return t;
                }).ToList();
            }
            if (localDirList.Any())
            {
                package.CriFileList = package.CriFileList.Join(localDirList, t => t.FileId, ld => ld.Id, (t, ld) =>
                {
                    
                    t.LocalDir = ld.str;
                    return t;
                }).ToList();
            }
            ///////
            ///
            return true;
        }
    }
}
