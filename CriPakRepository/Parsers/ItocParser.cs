using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CriPakInterfaces;
using CriPakRepository.Helpers;

namespace CriPakRepository.Parsers
{
    public class ItocParser : Parser<IEndianReader>, IItocParser<IEndianReader>
    {
        public bool Parse(IEndianReader br, ulong startoffset, ulong ContentOffset, ushort Align)
        {
            br.BaseStream.Seek((long)startoffset, SeekOrigin.Begin);

            if (br.ReadCString(4) != "ITOC")
            {
                br.Close();
                return false;
            }

            ReadUTFData(br);

            ITOC_packet = utf_packet;

            FileEntry itoc_entry = FileTable.Where(x => x.FileName.ToString() == "ITOC_HDR").Single();
            itoc_entry.Encrypted = isUtfEncrypted;
            itoc_entry.FileSize = ITOC_packet.Length;

            if (!ReadDataRows())
            {
                return false;
            }

            //uint FilesL = (uint)GetColumnData(files, 0, "FilesL");
            //uint FilesH = (uint)GetColumnData(files, 0, "FilesH");
            byte[] DataL = (byte[])GetColumnData(files, 0, "DataL");
            long DataLPos = GetColumnPostion(files, 0, "DataL");

            byte[] DataH = (byte[])GetColumnData(files, 0, "DataH");
            long DataHPos = GetColumnPostion(files, 0, "DataH");

            //MemoryStream ms;
            //EndianReader ir;
            UTF utfDataL, utfDataH;
            Dictionary<int, uint> SizeTable, CSizeTable;
            Dictionary<int, long> SizePosTable, CSizePosTable;
            Dictionary<int, Type> SizeTypeTable, CSizeTypeTable;

            List<int> IDs = new List<int>();

            SizeTable = new Dictionary<int, uint>();
            SizePosTable = new Dictionary<int, long>();
            SizeTypeTable = new Dictionary<int, Type>();

            CSizeTable = new Dictionary<int, uint>();
            CSizePosTable = new Dictionary<int, long>();
            CSizeTypeTable = new Dictionary<int, Type>();

            ushort ID, size1;
            uint size2;
            long pos;
            Type type;

            if (DataL != null)
            {
                var utfr = new EndianReader<MemoryStream, EndianData>(new MemoryStream(DataL), new EndianData(false));
                utfDataL = new UTF(tools);
                utfDataL.ReadUTF(utfr);

                for (int i = 0; i < utfDataL.num_rows; i++)
                {
                    ID = (ushort)GetColumnData(utfDataL, i, "ID");
                    size1 = (ushort)GetColumnData(utfDataL, i, "FileSize");
                    SizeTable.Add((int)ID, (uint)size1);

                    pos = GetColumnPostion(utfDataL, i, "FileSize");
                    SizePosTable.Add((int)ID, pos + DataLPos);

                    type = GetColumnType(utfDataL, i, "FileSize");
                    SizeTypeTable.Add((int)ID, type);

                    if ((GetColumnData(utfDataL, i, "ExtractSize")) != null)
                    {
                        size1 = (ushort)GetColumnData(utfDataL, i, "ExtractSize");
                        CSizeTable.Add((int)ID, (uint)size1);

                        pos = GetColumnPostion(utfDataL, i, "ExtractSize");
                        CSizePosTable.Add((int)ID, pos + DataLPos);

                        type = GetColumnType(utfDataL, i, "ExtractSize");
                        CSizeTypeTable.Add((int)ID, type);
                    }

                    IDs.Add(ID);
                }
            }

            if (DataH != null)
            {
                var utfr = new EndianReader<MemoryStream, EndianData>(new MemoryStream(DataH), new EndianData(false));
                utfDataH = new UTF(tools);
                utfDataH.ReadUTF(utfr);

                for (int i = 0; i < utfDataH.num_rows; i++)
                {
                    ID = (ushort)GetColumnData(utfDataH, i, "ID");
                    size2 = (uint)GetColumnData(utfDataH, i, "FileSize");
                    SizeTable.Add(ID, size2);

                    pos = GetColumnPostion(utfDataH, i, "FileSize");
                    SizePosTable.Add((int)ID, pos + DataHPos);

                    type = GetColumnType(utfDataH, i, "FileSize");
                    SizeTypeTable.Add((int)ID, type);

                    if ((GetColumnData(utfDataH, i, "ExtractSize")) != null)
                    {
                        size2 = (uint)GetColumnData(utfDataH, i, "ExtractSize");
                        CSizeTable.Add(ID, size2);

                        pos = GetColumnPostion(utfDataH, i, "ExtractSize");
                        CSizePosTable.Add((int)ID, pos + DataHPos);

                        type = GetColumnType(utfDataH, i, "ExtractSize");
                        CSizeTypeTable.Add((int)ID, type);
                    }

                    IDs.Add(ID);
                }
            }

            FileEntry temp;
            //int id = 0;
            uint value = 0, value2 = 0;
            ulong baseoffset = ContentOffset;

            // Seems ITOC can mix up the IDs..... but they'll alwaysy be in order...
            IDs = IDs.OrderBy(x => x).ToList();


            for (int i = 0; i < IDs.Count; i++)
            {
                int id = IDs[i];

                temp = new FileEntry();
                SizeTable.TryGetValue(id, out value);
                CSizeTable.TryGetValue(id, out value2);

                temp.TOCName = "ITOC";

                temp.DirName = null;
                temp.FileName = id.ToString() + ".bin";

                temp.FileSize = value;
                temp.FileSizePos = SizePosTable[id];
                temp.FileSizeType = SizeTypeTable[id];

                if (CSizeTable.Count > 0 && CSizeTable.ContainsKey(id))
                {
                    temp.ExtractSize = value2;
                    temp.ExtractSizePos = CSizePosTable[id];
                    temp.ExtractSizeType = CSizeTypeTable[id];
                }

                temp.FileType = "FILE";


                temp.FileOffset = baseoffset;
                temp.ID = id;
                temp.UserString = null;

                FileTable.Add(temp);

                if ((value % Align) > 0)
                    baseoffset += value + (Align - (value % Align));
                else
                    baseoffset += value;


                //id++;
            }

            files = null;
            utfDataL = null;
            utfDataH = null;
            return true;
        }


    }
}
