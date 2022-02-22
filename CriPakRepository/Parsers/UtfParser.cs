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
        public override bool Parse(CriPak package) //, Encoding encoding = null)
        {
            package.Utf = new UTF();
            long offset = package.SubReader.BaseStream.Position;

            if (package.SubReader.ReadCString(4) != "@UTF")
            {
                return false;
            }

            package.Utf.TableSize = package.SubReader.ReadInt32();
            package.Utf.RowsOffeset = package.SubReader.ReadInt32();
            package.Utf.StringsOffset = package.SubReader.ReadInt32();
            package.Utf.DataOffset = package.SubReader.ReadInt32();

            // CPK Header & UTF Header are ignored, so add 8 to each offset
            package.Utf.RowsOffeset += (offset + 8);
            package.Utf.StringsOffset += (offset + 8);
            package.Utf.DataOffset += (offset + 8);

            package.Utf.TableName = package.SubReader.ReadInt32();
            package.Utf.NumColumns = package.SubReader.ReadInt16();
            package.Utf.RowLength = package.SubReader.ReadInt16();
            package.Utf.NumRows = package.SubReader.ReadInt32();

            //read Columns
            for (int i = 0; i < package.Utf.NumColumns; i++)
            {
                var column = new Column();
                column.Flags = package.SubReader.ReadByte();
                if (column.Flags == 0)
                {
                    package.SubReader.BaseStream.Seek(3, SeekOrigin.Current);
                    column.Flags = package.SubReader.ReadByte();
                }

                column.Name = package.SubReader.ReadCString(-1, (long)(package.SubReader.ReadInt32() + package.Utf.StringsOffset), package.Encoding);
                package.Utf.Columns.Add(column);
            }

            //read Rows
            // Originally this called for a List<List<Row>>. I dont see a need for a nested list as its always hardcorded to the 1st element, so its been removed.
            // If Num Rows goes above 1 something will need added to deal with different rows or this process will need refactoring somehow.  
            for (int j = 0; j < package.Utf.NumRows; j++)
            {
                package.SubReader.BaseStream.Seek(package.Utf.RowsOffeset + (j * package.Utf.RowLength), SeekOrigin.Begin);

                for (int i = 0; i < package.Utf.NumColumns; i++)
                {
                    var row = new Row();
                    var storage_flag = (package.Utf.Columns[i].Flags & (int)STORAGE.MASK);
                    if (!(storage_flag == (int)STORAGE.NONE || storage_flag == (int)STORAGE.ZERO || storage_flag == (int)STORAGE.CONSTANT))
                    {
                        row.Id = j+1;
                        row.Type = package.Utf.Columns[i].Flags & (int)CRITYPE.MASK;
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
                                row.str = package.SubReader.ReadCString(-1, package.SubReader.ReadInt32() + package.Utf.StringsOffset, package.Encoding);
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
