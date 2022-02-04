using CriPakInterfaces;
using CriPakRepository.Helpers;
using System.IO;
using System.Linq;

namespace CriPakRepository.Parsers
{
    public class GtocParser : Parser<IEndianReader>
    {
        public bool Parse(IEndianReader br, ulong startoffset)
        {
            br.BaseStream.Seek((long)startoffset, SeekOrigin.Begin);

            if (br.ReadCString(4) != "GTOC")
            {
                br.Close();
                return false;
            }

            //br.BaseStream.Seek(0xC, SeekOrigin.Current); //skip header data
            ReadUTFData(br);

            GTOC_packet = utf_packet;
            FileEntry gtoc_entry = FileTable.Where(x => x.FileName.ToString() == "GTOC_HDR").Single();
            gtoc_entry.Encrypted = isUtfEncrypted;
            gtoc_entry.FileSize = GTOC_packet.Length;


            return true;
        }


    }
}
