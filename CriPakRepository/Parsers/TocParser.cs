using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Parsers
{
    //public class TocParser : ParserRepository
    //{
    //    public override bool Parse(CriPak package) //, ulong TocOffset, ulong contentOffset, Encoding encoding = null)
    //    {
    //        ulong fTocOffset = TocOffset;
    //        ulong add_offset = 0;

    //        if (fTocOffset > (ulong)0x800)
    //            fTocOffset = (ulong)0x800;


    //        if (ContentOffset < 0)
    //            add_offset = fTocOffset;
    //        else
    //        {
    //            if (TocOffset < 0)
    //                add_offset = ContentOffset;
    //            else
    //            {
    //                if (ContentOffset < fTocOffset)
    //                    add_offset = ContentOffset;
    //                else
    //                    add_offset = fTocOffset;
    //            }
    //        }

    //        package.Reader.BaseStream.Seek((long)TocOffset, SeekOrigin.Begin);

    //        if (package.Reader.ReadCString(4) != "TOC ")
    //        {
    //            package.Reader.Close();
    //            return false;
    //        }

    //        ReadUTFData(package.Reader);

    //        // Store unencrypted TOC
    //        TOC_packet = utf_packet;

    //        FileEntry toc_entry = FileTable.Where(x => x.FileName.ToString() == "TOC_HDR").Single();
    //        toc_entry.Encrypted = isUtfEncrypted;
    //        toc_entry.FileSize = TOC_packet.Length;

    //        if (!ReadDataRows(encoding))
    //        {
    //            return false;
    //        }
    //        //This is involved in ReadDataRows and the GetColumnar methods below.  must be resolved.  
    //        var files = new UTF();

    //        for (int i = 0; i < files.NumRows; i++)
    //        {
    //            var temp = new CriFile();

    //            temp.TOCName = "TOC";

    //            temp.DirName = files.GetRow("DirName")?.GetValue().ToString();
    //            temp.FileName = files.GetRow("FileName")?.GetValue().ToString();

    //            temp.FileSize = int.TryParse(files.GetRow("FileSize")?.GetValue().ToString(), out var num) ? num : 0;
    //            temp.FileSizePos = files.GetPostion("FileSize");
    //            temp.FileSizeType = files.GetRow("FileSize")?.GetType();

    //            temp.ExtractedFileSize= int.TryParse(files.GetRow("ExtractSize")?.GetValue().ToString(), out num) ? num : 0;
    //            temp.ExtractSizePos = files.GetPostion("ExtractSize");
    //            temp.ExtractSizeType = files.GetRow("ExtractSize")?.GetType();

    //            temp.FileOffset = (ulong)files.GetRow("FileOffset")?.GetValue() + add_offset;
    //            temp.FileOffsetPos = files.GetPostion("FileOffset");
    //            temp.FileOffsetType = files.GetRow("FileOffset")?.GetType();

    //            temp.FileType = "FILE";

    //            temp.Offset = add_offset;

    //            temp.ID = int.TryParse(files.GetRow("ID")?.GetValue().ToString(), out num) ? num : 0;
    //            temp.UserString = files.GetRow("UserString")?.GetValue().ToString();

    //            FileTable.Add(temp);
    //        }
    //        files = null;

    //        return true;
    //    }
    //}
}
