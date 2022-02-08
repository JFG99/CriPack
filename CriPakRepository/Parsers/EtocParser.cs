using CriPakInterfaces;
using CriPakRepository.Helpers;
using CriPakRepository.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CriPakRepository.Parsers
{
    public class EtocParser : Parser<IEndianReader>
    {
        public new bool Parse(IEndianReader br, ulong startOffset)
        {
            //Move this to parent
            br.BaseStream.Seek((long)startOffset, SeekOrigin.Begin);

            if (br.ReadCString(4) != "ETOC")
            {
                br.Close();
                return false;
            }          
            //br.BaseStream.Seek(0xC, SeekOrigin.Current); //skip header data

            ReadUTFData(br);

            ETOC_packet = utf_packet;

            FileEntry etoc_entry = FileTable.Where(x => x.FileName.ToString() == "ETOC_HDR").Single();
            etoc_entry.Encrypted = isUtfEncrypted;
            etoc_entry.FileSize = ETOC_packet.Length;

            if (!ReadDataRows())
            {
                return false;
            }

            List<FileEntry> fileEntries = FileTable.Where(x => x.FileType == "FILE").ToList();

            for (int i = 0; i < fileEntries.Count; i++)
            {
                FileTable[i].LocalDir = GetColumnData(files, i, "LocalDir");
                FileTable[i].UpdateDateTime = (ulong)GetColumnData(files, i, "UpdateDateTime");
            }

            return true;
        }
    }
}
