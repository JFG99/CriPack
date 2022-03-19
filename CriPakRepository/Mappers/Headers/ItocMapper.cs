using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakRepository.Mappers
{
    public class ItocMapper : IDetailMapper<ItocHeader>
    {
        public ItocHeader Map(IEntity header, IEnumerable<CriPakInterfaces.Models.ComponentsNew.Row> rowValue)
        {
            //Not Currently Implemented. Old code is below.

            return new ItocHeader()
            {
            };
        }
    }
}


//    package.Reader.BaseStream.Seek((long)package.ItocOffset, SeekOrigin.Begin);

//    if (package.Reader.ReadCString(4) != "ITOC")
//    {
//        package.Reader.Close();
//        return false;
//    }

//    package.ReadUTFData();
//    package.ItocPacket = package.UtfPacket;

//    if (!package.ReadDataRows())
//    {
//        return false;
//    }

//    //uint FilesL = (uint)GetColumnData(files, 0, "FilesL");
//    //uint FilesH = (uint)GetColumnData(files, 0, "FilesH");

//    byte[] DataL = (byte[])package.Utf.GetRowValue("DataL");
//    long DataLPos = package.Utf.GetRowPostion("DataL");

//byte[] DataH = (byte[])GetColumnData(files, 0, "DataH");
// long DataHPos = GetColumnPostion(files, 0, "DataH");

//MemoryStream ms;
//EndianReader ir;
//UTF utfDataL, utfDataH;
//Dictionary<int, uint> SizeTable, CSizeTable;
//Dictionary<int, long> SizePosTable, CSizePosTable;
//Dictionary<int, Type> SizeTypeTable, CSizeTypeTable;

//List<int> IDs = new List<int>();

//SizeTable = new Dictionary<int, uint>();
//SizePosTable = new Dictionary<int, long>();
//SizeTypeTable = new Dictionary<int, Type>();

//CSizeTable = new Dictionary<int, uint>();
//CSizePosTable = new Dictionary<int, long>();
//CSizeTypeTable = new Dictionary<int, Type>();

//ushort ID, size1;
//uint size2;
//long pos;
//Type type;

//if (DataL != null)
//{
//    package.SubReader = new EndianReader<MemoryStream, EndianData>(new MemoryStream(DataL), new EndianData(false));
//    var parser = new UtfParser();
//    parser.Parse(package);

//    for (int i = 0; i < utfDataL.num_rows; i++)
//    {
//        ID = (ushort)GetColumnData(utfDataL, i, "ID");
//        size1 = (ushort)GetColumnData(utfDataL, i, "FileSize");
//        SizeTable.Add((int)ID, (uint)size1);

//        pos = GetColumnPostion(utfDataL, i, "FileSize");
//        SizePosTable.Add((int)ID, pos + DataLPos);

//        type = GetColumnType(utfDataL, i, "FileSize");
//        SizeTypeTable.Add((int)ID, type);

//        if ((GetColumnData(utfDataL, i, "ExtractSize")) != null)
//        {
//            size1 = (ushort)GetColumnData(utfDataL, i, "ExtractSize");
//            CSizeTable.Add((int)ID, (uint)size1);

//            pos = GetColumnPostion(utfDataL, i, "ExtractSize");
//            CSizePosTable.Add((int)ID, pos + DataLPos);

//            type = GetColumnType(utfDataL, i, "ExtractSize");
//            CSizeTypeTable.Add((int)ID, type);
//        }

//        IDs.Add(ID);
//    }
//}

//        if (DataH != null)
//        {
//            var utfr = new EndianReader<MemoryStream, EndianData>(new MemoryStream(DataH), new EndianData(false));
//            utfDataH = new UTF(tools);
//            utfDataH.ReadUTF(utfr);

//            for (int i = 0; i < utfDataH.num_rows; i++)
//            {
//                ID = (ushort)GetColumnData(utfDataH, i, "ID");
//                size2 = (uint)GetColumnData(utfDataH, i, "FileSize");
//                SizeTable.Add(ID, size2);

//                pos = GetColumnPostion(utfDataH, i, "FileSize");
//                SizePosTable.Add((int)ID, pos + DataHPos);

//                type = GetColumnType(utfDataH, i, "FileSize");
//                SizeTypeTable.Add((int)ID, type);

//                if ((GetColumnData(utfDataH, i, "ExtractSize")) != null)
//                {
//                    size2 = (uint)GetColumnData(utfDataH, i, "ExtractSize");
//                    CSizeTable.Add(ID, size2);

//                    pos = GetColumnPostion(utfDataH, i, "ExtractSize");
//                    CSizePosTable.Add((int)ID, pos + DataHPos);

//                    type = GetColumnType(utfDataH, i, "ExtractSize");
//                    CSizeTypeTable.Add((int)ID, type);
//                }

//                IDs.Add(ID);
//            }
//        }

//        FileEntry temp;
//        //int id = 0;
//        uint value = 0, value2 = 0;
//        ulong baseoffset = ContentOffset;

//        // Seems ITOC can mix up the IDs..... but they'll alwaysy be in order...
//        IDs = IDs.OrderBy(x => x).ToList();


//        for (int i = 0; i < IDs.Count; i++)
//        {
//            int id = IDs[i];

//            temp = new FileEntry();
//            SizeTable.TryGetValue(id, out value);
//            CSizeTable.TryGetValue(id, out value2);

//            temp.TOCName = "ITOC";

//            temp.DirName = null;
//            temp.FileName = id.ToString() + ".bin";

//            temp.FileSize = value;
//            temp.FileSizePos = SizePosTable[id];
//            temp.FileSizeType = SizeTypeTable[id];

//            if (CSizeTable.Count > 0 && CSizeTable.ContainsKey(id))
//            {
//                temp.ExtractedFileSize= value2;
//                temp.ExtractSizePos = CSizePosTable[id];
//                temp.ExtractSizeType = CSizeTypeTable[id];
//            }

//            temp.FileType = "FILE";


//            temp.FileOffset = baseoffset;
//            temp.ID = id;
//            temp.UserString = null;

//            FileTable.Add(temp);

//            if ((value % Align) > 0)
//                baseoffset += value + (Align - (value % Align));
//            else
//                baseoffset += value;


//            //id++;
//        }

//files = null;
//utfDataL = null;
//utfDataH = null;
//    return true;
//}
