using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace CriPakRepository.Parsers
{
    public class UtfParser : ParserRepository
    {
        public override bool Parse(CriPak package) 
        {
            package.Utf = new UTF();
            long offset = package.SubReader.BaseStream.Position;

            if (package.SubReader.ReadCString(4) != "@UTF")
            {
                return false;
            }

            package.Utf.TableSize = package.SubReader.ReadInt32();
            // CPK Header & UTF Header are ignored, so add 8 to each offset
            package.Utf.RowsOffeset = package.SubReader.ReadInt32() + (offset + 8);
            package.Utf.StringsOffset = package.SubReader.ReadInt32() + (offset + 8);
            package.Utf.DataOffset = package.SubReader.ReadInt32() + (offset + 8);
            package.Utf.TableName = package.SubReader.ReadInt32();
            package.Utf.NumColumns = package.SubReader.ReadInt16();
            package.Utf.RowLength = package.SubReader.ReadInt16();
            package.Utf.NumRows = package.SubReader.ReadInt32();

            //Read Column Details
            var columnBytes = package.SubReader.ReadBytes(package.Utf.NumColumns * 5).ToList();
            var skip = columnBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);//locate block data where byte 0 == 0
            while (skip > -1)//iterate and delete 4 byte blocks where byte[0] == 0, then append 4 more bytes each time
            {                            
                columnBytes.RemoveRange(skip * 5, 4);
                columnBytes.AddRange(package.SubReader.ReadBytes(4));               
                skip = columnBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);
            }//This gets us a corrected byte header list where byte[0] is flag and byte 1-4 is padding size.
            package.Utf.Columns = columnBytes.Select((x, i) =>
                new { Index = i, Value = x })
                .GroupBy(x => x.Index / 5)
                .Select(x =>
                {
                    var bytes = x.Select(v => v.Value).ToArray();
                    return new Column()
                    {           
                        Flag = bytes[0],
                        Name = package.SubReader.ReadCString(BitConverter.ToInt32(bytes.Skip(1).Take(4).Reverse().ToArray(), 0) + package.Utf.StringsOffset, package.Encoding)
                    };
                }).ToList();

            //read Rows
            // Originally this called for a List<List<Row>>. I dont see a need for a nested list as its always hardcorded to the 1st element, so its been removed.
            // If Num Rows goes above 1 something will need added to deal with different rows or this process will need refactoring somehow.  
            for (int j = 0; j < package.Utf.NumRows; j++)
            {
                package.SubReader.BaseStream.Seek(package.Utf.RowsOffeset + (j * package.Utf.RowLength), SeekOrigin.Begin);

                for (int i = 0; i < package.Utf.NumColumns; i++)
                {
                    var row = new Row();
                    var storage_flag = (package.Utf.Columns[i].Flag & (int)STORAGE.MASK);
                    if (!(storage_flag == (int)STORAGE.NONE || storage_flag == (int)STORAGE.ZERO || storage_flag == (int)STORAGE.CONSTANT))
                    {
                        row.Id = j+1;
                        row.Type = package.Utf.Columns[i].Flag & (int)CRITYPE.MASK;
                        row.Position = package.SubReader.BaseStream.Position;
                        row.Name = package.Utf.Columns[i].Name;
                        //Bleh switch statements. fix if time.
                        switch (row.Type)
                        {
                            case 0:
                            case 1:
                                row.uint8 = package.SubReader.ReadByte();
                                break;

                            case 2:
                            case 3:
                                row.uint16 = package.SubReader.ReadUInt16();
                                break;

                            case 4:
                            case 5:
                                row.uint32 = package.SubReader.ReadUInt32();
                                break;

                            case 6:
                            case 7:
                                row.uint64 = package.SubReader.ReadUInt64();
                                break;

                            case 8:
                                row.ufloat = package.SubReader.ReadSingle();
                                break;

                            case 0xA:
                                row.str = package.SubReader.ReadCString(package.SubReader.ReadInt32() + package.Utf.StringsOffset, package.Encoding);
                                break;

                            case 0xB:
                                row.Position = package.SubReader.ReadInt32() + package.Utf.DataOffset;
                                row.data = package.SubReader.GetData(row.Position, package.SubReader.ReadInt32());
                                break;

                            default: throw new NotImplementedException();
                        }
                    }
                    package.Utf.Rows.Add(row);
                    if (!package.CpkData.ContainsKey(package.Utf.Columns[i].Name))
                    {
                        package.CpkData.Add(package.Utf.Columns[i].Name, row.GetValue());
                    }
                }
            }
            return true;
        }
    }
}
