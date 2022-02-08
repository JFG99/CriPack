using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakRepository.Helpers
{
    public static class UtfExtensions
    {
        public static  object GetColumsData2(this UTF utf, int row, string Name, int type)
        {
            object Temp = utf.GetRow(Name)?.GetValue();

            if (Temp == null)
            {
                switch (type)
                {
                    case 0: // byte
                        return (byte)0xFF;
                    case 1: // short
                        return (ushort)0xFFFF;
                    case 2: // int
                        return 0xFFFFFFFF;
                    case 3: // long
                        return 0xFFFFFFFFFFFFFFFF;
                }
            }

            if (Temp is ulong)
            {
                return (Temp == null) ? 0xFFFFFFFFFFFFFFFF : (ulong)Temp;
            }

            if (Temp is uint)
            {
                return (Temp == null) ? 0xFFFFFFFF : (uint)Temp;
            }

            if (Temp is ushort)
            {
                return (Temp == null) ? (ushort)0xFFFF : (ushort)Temp;
            }

            return 0;
        }                
                
        public static long GetPostion(this UTF utf, string Name)
        {
            return utf.GetRow(Name)?.Position ?? -1;
        }                

        public static Row GetRow(this UTF utf, string Name)
        {
            try
            {
                return utf.Rows[utf.Columns.FindIndex(x => x.Name == Name)];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
