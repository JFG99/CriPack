using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriPakRepository.Parsers
{
    public class CpkParser : Parser<IEndianReader>, ICpkParser
    {
        public bool Parse(string path, Encoding encoding = null)
        {
            if (File.Exists(path))
            {
                uint Files;
                ushort Align;
                var test = new EndianData(true);
                var br = new EndianReader<FileStream, EndianData>(File.OpenRead(path), test);

                if (br.ReadCString(4) != "CPK ")
                {
                    br.Close();
                    return false;
                }

                br.ReadUTFData();

                CPK_packet = utf_packet;

                var CPAK_entry = new CriFile
                {
                    FileName = "CPK_HDR",
                    FileOffsetPos = br.BaseStream.Position + 0x10,
                    FileSize = CPK_packet.Length,
                    Encrypted = isUtfEncrypted,
                    FileType = "CPK"
                };

                FileTable.Add(CPAK_entry);

                if (!ReadDataRows(encoding))
                {
                    return false;
                }

                cpkdata = new Dictionary<string, object>();

                try
                {
                    for (int i = 0; i < utf.columns.Count; i++)
                    {
                        cpkdata.Add(utf.columns[i].name, utf.rows[0].rows[i].GetValue());
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    Console.WriteLine(ex.ToString());
                }

                TocOffset = (ulong)GetColumsData2(utf, 0, "TocOffset", 3);
                long TocOffsetPos = GetColumnPostion(utf, 0, "TocOffset");

                EtocOffset = (ulong)GetColumsData2(utf, 0, "EtocOffset", 3);
                long ETocOffsetPos = GetColumnPostion(utf, 0, "EtocOffset");

                ItocOffset = (ulong)GetColumsData2(utf, 0, "ItocOffset", 3);
                long ITocOffsetPos = GetColumnPostion(utf, 0, "ItocOffset");

                GtocOffset = (ulong)GetColumsData2(utf, 0, "GtocOffset", 3);
                long GTocOffsetPos = GetColumnPostion(utf, 0, "GtocOffset");

                ContentOffset = (ulong)GetColumsData2(utf, 0, "ContentOffset", 3);
                long ContentOffsetPos = GetColumnPostion(utf, 0, "ContentOffset");
                FileTable.Add(new CriFile("CONTENT_OFFSET", ContentOffset, typeof(ulong), ContentOffsetPos, "CPK", "CONTENT", false));

                Files = (uint)GetColumsData2(utf, 0, "Files", 2);
                Align = (ushort)GetColumsData2(utf, 0, "Align", 1);

                if (TocOffset != 0xFFFFFFFFFFFFFFFF)
                {
                    CriFile entry = new CriFile("TOC_HDR", TocOffset, typeof(ulong), TocOffsetPos, "CPK", "HDR", false);
                    FileTable.Add(entry);

                    if (!ReadTOC(br, TocOffset, ContentOffset, encoding))
                        return false;
                }

                if (EtocOffset != 0xFFFFFFFFFFFFFFFF)
                {
                    CriFile entry = new CriFile("ETOC_HDR", EtocOffset, typeof(ulong), ETocOffsetPos, "CPK", "HDR", false);
                    FileTable.Add(entry);

                    if (!ReadETOC(br, EtocOffset))
                        return false;
                }

                if (ItocOffset != 0xFFFFFFFFFFFFFFFF)
                {
                    //CriFile ITOC_entry = new CriFile { 
                    //    FileName = "ITOC_HDR",
                    //    FileOffset = ItocOffset, FileOffsetType = typeof(ulong), FileOffsetPos = ITocOffsetPos,
                    //    TOCName = "CPK",
                    //    FileType = "FILE", Encrypted = true,
                    //};

                    CriFile entry = new CriFile("ITOC_HDR", ItocOffset, typeof(ulong), ITocOffsetPos, "CPK", "HDR", false);
                    FileTable.Add(entry);

                    if (!ReadITOC(br, ItocOffset, ContentOffset, Align))
                        return false;
                }

                if (GtocOffset != 0xFFFFFFFFFFFFFFFF)
                {
                    CriFile entry = new CriFile("GTOC_HDR", GtocOffset, typeof(ulong), GTocOffsetPos, "CPK", "HDR", false);
                    FileTable.Add(entry);

                    if (!ReadGTOC(br, GtocOffset))
                        return false;
                }

                br.Close();

                // at this point, we should have all needed file info

                //utf = null;
                files = null;
                return true;
            }
            return false;
        }
    }
}
