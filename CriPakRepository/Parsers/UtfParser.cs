using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakRepository.Helpers;
using CriPakRepository.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriPakRepository.Parsers
{
    //public class UtfParser : ParserRepository
    //{
    //    public override bool Parse(CriPak package) //, Encoding encoding = null)
    //    {
    //        long offset = package.Reader.BaseStream.Position;

    //        if (package.Reader.ReadCString(4) != "@UTF")
    //        {
    //            return false;
    //        }

    //        table_size = package.Reader.ReadInt32();
    //        rows_offset = package.Reader.ReadInt32();
    //        strings_offset = package.Reader.ReadInt32();
    //        data_offset = package.Reader.ReadInt32();

    //        // CPK Header & UTF Header are ignored, so add 8 to each offset
    //        rows_offset += (offset + 8);
    //        strings_offset += (offset + 8);
    //        data_offset += (offset + 8);

    //        table_name = package.Reader.ReadInt32();
    //        num_columns = package.Reader.ReadInt16();
    //        row_length = package.Reader.ReadInt16();
    //        num_rows = package.Reader.ReadInt32();

    //        //read Columns
    //        columns = new List<COLUMN>();
    //        COLUMN column;

    //        for (int i = 0; i < num_columns; i++)
    //        {
    //            column = new COLUMN();
    //            column.flags = package.Reader.ReadByte();
    //            if (column.flags == 0)
    //            {
    //                package.Reader.BaseStream.Seek(3, SeekOrigin.Current);
    //                column.flags = package.Reader.ReadByte();
    //            }

    //            column.name = package.Reader.ReadCString(-1, (long)(package.Reader.ReadInt32() + strings_offset), encoding);
    //            columns.Add(column);
    //        }

    //        //read Rows

    //        rows = new List<ROWS>();
    //        ROWS current_entry;
    //        ROW current_row;
    //        int storage_flag;

    //        for (int j = 0; j < num_rows; j++)
    //        {
    //            package.Reader.BaseStream.Seek(rows_offset + (j * row_length), SeekOrigin.Begin);

    //            current_entry = new ROWS();

    //            for (int i = 0; i < num_columns; i++)
    //            {
    //                current_row = new ROW();

    //                storage_flag = (columns[i].flags & (int)COLUMN_FLAGS.STORAGE_MASK);

    //                if (storage_flag == (int)COLUMN_FLAGS.STORAGE_NONE) // 0x00
    //                {
    //                    current_entry.rows.Add(current_row);
    //                    continue;
    //                }

    //                if (storage_flag == (int)COLUMN_FLAGS.STORAGE_ZERO) // 0x10
    //                {
    //                    current_entry.rows.Add(current_row);
    //                    continue;
    //                }

    //                if (storage_flag == (int)COLUMN_FLAGS.STORAGE_CONSTANT) // 0x30
    //                {
    //                    current_entry.rows.Add(current_row);
    //                    continue;
    //                }

    //                // 0x50

    //                current_row.type = columns[i].flags & (int)COLUMN_FLAGS.TYPE_MASK;

    //                current_row.position = package.Reader.BaseStream.Position;

    //                switch (current_row.type)
    //                {
    //                    case 0:
    //                    case 1:
    //                        current_row.uint8 = package.Reader.ReadByte();
    //                        package.Readereak;

    //                    case 2:
    //                    case 3:
    //                        current_row.uint16 = package.Reader.ReadUInt16();
    //                        package.Readereak;

    //                    case 4:
    //                    case 5:
    //                        current_row.uint32 = package.Reader.ReadUInt32();
    //                        package.Readereak;

    //                    case 6:
    //                    case 7:
    //                        current_row.uint64 = package.Reader.ReadUInt64();
    //                        package.Readereak;

    //                    case 8:
    //                        current_row.ufloat = package.Reader.ReadSingle();
    //                        package.Readereak;

    //                    case 0xA:
    //                        current_row.str = package.Reader.ReadCString(-1, package.Reader.ReadInt32() + strings_offset, encoding);
    //                        package.Readereak;

    //                    case 0xB:
    //                        long position = package.Reader.ReadInt32() + data_offset;
    //                        current_row.position = position;
    //                        current_row.data = package.Reader.GetData(position, package.Reader.ReadInt32());
    //                        package.Readereak;

    //                    default: throw new NotImplementedException();
    //                }


    //                current_entry.rows.Add(current_row);
    //            }

    //            rows.Add(current_entry);
    //        }

    //        return true;
    //    }
    //}
}
