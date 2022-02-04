using CriPakInterfaces;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriPakRepository.Parsers
{
    public class UtfParser : Parser<IEndianReader>, IUtfParser<IEndianReader>
    {
        public bool Parse(IEndianReader br, Encoding encoding = null)
        {
            long offset = br.BaseStream.Position;

            if (br.ReadCString(4) != "@UTF")
            {
                return false;
            }

            table_size = br.ReadInt32();
            rows_offset = br.ReadInt32();
            strings_offset = br.ReadInt32();
            data_offset = br.ReadInt32();

            // CPK Header & UTF Header are ignored, so add 8 to each offset
            rows_offset += (offset + 8);
            strings_offset += (offset + 8);
            data_offset += (offset + 8);

            table_name = br.ReadInt32();
            num_columns = br.ReadInt16();
            row_length = br.ReadInt16();
            num_rows = br.ReadInt32();

            //read Columns
            columns = new List<COLUMN>();
            COLUMN column;

            for (int i = 0; i < num_columns; i++)
            {
                column = new COLUMN();
                column.flags = br.ReadByte();
                if (column.flags == 0)
                {
                    br.BaseStream.Seek(3, SeekOrigin.Current);
                    column.flags = br.ReadByte();
                }

                column.name = br.ReadCString(-1, (long)(br.ReadInt32() + strings_offset), encoding);
                columns.Add(column);
            }

            //read Rows

            rows = new List<ROWS>();
            ROWS current_entry;
            ROW current_row;
            int storage_flag;

            for (int j = 0; j < num_rows; j++)
            {
                br.BaseStream.Seek(rows_offset + (j * row_length), SeekOrigin.Begin);

                current_entry = new ROWS();

                for (int i = 0; i < num_columns; i++)
                {
                    current_row = new ROW();

                    storage_flag = (columns[i].flags & (int)COLUMN_FLAGS.STORAGE_MASK);

                    if (storage_flag == (int)COLUMN_FLAGS.STORAGE_NONE) // 0x00
                    {
                        current_entry.rows.Add(current_row);
                        continue;
                    }

                    if (storage_flag == (int)COLUMN_FLAGS.STORAGE_ZERO) // 0x10
                    {
                        current_entry.rows.Add(current_row);
                        continue;
                    }

                    if (storage_flag == (int)COLUMN_FLAGS.STORAGE_CONSTANT) // 0x30
                    {
                        current_entry.rows.Add(current_row);
                        continue;
                    }

                    // 0x50

                    current_row.type = columns[i].flags & (int)COLUMN_FLAGS.TYPE_MASK;

                    current_row.position = br.BaseStream.Position;

                    switch (current_row.type)
                    {
                        case 0:
                        case 1:
                            current_row.uint8 = br.ReadByte();
                            break;

                        case 2:
                        case 3:
                            current_row.uint16 = br.ReadUInt16();
                            break;

                        case 4:
                        case 5:
                            current_row.uint32 = br.ReadUInt32();
                            break;

                        case 6:
                        case 7:
                            current_row.uint64 = br.ReadUInt64();
                            break;

                        case 8:
                            current_row.ufloat = br.ReadSingle();
                            break;

                        case 0xA:
                            current_row.str = br.ReadCString(-1, br.ReadInt32() + strings_offset, encoding);
                            break;

                        case 0xB:
                            long position = br.ReadInt32() + data_offset;
                            current_row.position = position;
                            current_row.data = br.GetData(position, br.ReadInt32());
                            break;

                        default: throw new NotImplementedException();
                    }


                    current_entry.rows.Add(current_row);
                }

                rows.Add(current_entry);
            }

            return true;
        }
    }
}
