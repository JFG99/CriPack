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
    //public class EtocParser : ParserRepository
    //{
    //    public override bool Parse(CriPak package)//, ulong startOffset)
    //    {
    //        //Move this to parent
    //        package.Reader.BaseStream.Seek((long)startOffset, SeekOrigin.Begin);

    //        if (package.Reader.ReadCString(4) != "ETOC")
    //        {
    //            package.Reader.Close();
    //            return false;
    //        }          
    //        //package.Reader.BaseStream.Seek(0xC, SeekOrigin.Current); //skip header data

    //        ReadUTFData(package.Reader);

    //        ETOC_packet = utf_packet;

    //        FileEntry etoc_entry = FileTable.Where(x => x.FileName.ToString() == "ETOC_HDR").Single();
    //        etoc_entry.Encrypted = isUtfEncrypted;
    //        etoc_entry.FileSize = ETOC_packet.Length;

    //        if (!ReadDataRows())
    //        {
    //            return false;
    //        }

    //        List<FileEntry> fileEntries = FileTable.Where(x => x.FileType == "FILE").ToList();

    //        for (int i = 0; i < fileEntries.Count; i++)
    //        {
    //            FileTable[i].LocalDir = GetColumnData(files, i, "LocalDir");
    //            FileTable[i].UpdateDateTime = (ulong)GetColumnData(files, i, "UpdateDateTime");
    //        }

    //        return true;
    //    }
    //}
}
