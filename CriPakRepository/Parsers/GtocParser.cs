using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using System.IO;
using System.Linq;

namespace CriPakRepository.Parsers
{
    //public class GtocParser : ParserRepository
    //{
    //    public override bool Parse(CriPak package)  //, ulong startoffset)
    //    {
    //        package.Reader.BaseStream.Seek((long)startoffset, SeekOrigin.Begin);

    //        if (package.Reader.ReadCString(4) != "GTOC")
    //        {
    //            package.Reader.Close();
    //            return false;
    //        }

    //        //package.Reader.BaseStream.Seek(0xC, SeekOrigin.Current); //skip header data
    //        ReadUTFData(package.Reader);

    //        GTOC_packet = utf_packet;
    //        FileEntry gtoc_entry = FileTable.Where(x => x.FileName.ToString() == "GTOC_HDR").Single();
    //        gtoc_entry.Encrypted = isUtfEncrypted;
    //        gtoc_entry.FileSize = GTOC_packet.Length;


    //        return true;
    //    }


    //}
}
