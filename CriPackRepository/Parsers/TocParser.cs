using CriPakInterfaces;
using CriPakRepository.Helpers;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Parsers
{
    public class TocParser : Parser<IEndianReader>, ITocParser<IEndianReader>
    {
        public bool Parse(IEndianReader br, ulong TocOffset, ulong contentOffset, Encoding encoding = null)
        {
            ulong fTocOffset = TocOffset;
            ulong add_offset = 0;

            if (fTocOffset > (ulong)0x800)
                fTocOffset = (ulong)0x800;


            if (ContentOffset < 0)
                add_offset = fTocOffset;
            else
            {
                if (TocOffset < 0)
                    add_offset = ContentOffset;
                else
                {
                    if (ContentOffset < fTocOffset)
                        add_offset = ContentOffset;
                    else
                        add_offset = fTocOffset;
                }
            }

            br.BaseStream.Seek((long)TocOffset, SeekOrigin.Begin);

            if (br.ReadCString(4) != "TOC ")
            {
                br.Close();
                return false;
            }

            ReadUTFData(br);

            // Store unencrypted TOC
            TOC_packet = utf_packet;

            FileEntry toc_entry = FileTable.Where(x => x.FileName.ToString() == "TOC_HDR").Single();
            toc_entry.Encrypted = isUtfEncrypted;
            toc_entry.FileSize = TOC_packet.Length;

            if (!ReadDataRows(encoding))
            {
                return false;
            }


            FileEntry temp;
            for (int i = 0; i < files.num_rows; i++)
            {
                temp = new FileEntry();

                temp.TOCName = "TOC";

                temp.DirName = GetColumnData(files, i, "DirName");
                temp.FileName = GetColumnData(files, i, "FileName");

                temp.FileSize = GetColumnData(files, i, "FileSize");
                temp.FileSizePos = GetColumnPostion(files, i, "FileSize");
                temp.FileSizeType = GetColumnType(files, i, "FileSize");

                temp.ExtractSize = GetColumnData(files, i, "ExtractSize");
                temp.ExtractSizePos = GetColumnPostion(files, i, "ExtractSize");
                temp.ExtractSizeType = GetColumnType(files, i, "ExtractSize");

                temp.FileOffset = ((ulong)GetColumnData(files, i, "FileOffset") + (ulong)add_offset);
                temp.FileOffsetPos = GetColumnPostion(files, i, "FileOffset");
                temp.FileOffsetType = GetColumnType(files, i, "FileOffset");

                temp.FileType = "FILE";

                temp.Offset = add_offset;

                temp.ID = GetColumnData(files, i, "ID");
                temp.UserString = GetColumnData(files, i, "UserString");

                FileTable.Add(temp);
            }
            files = null;

            return true;
        }
    }
}
