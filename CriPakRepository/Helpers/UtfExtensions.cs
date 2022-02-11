using CriPakInterfaces.Models.Components;
using System;
using System.Linq;
using System.Text;

namespace CriPakRepository.Helpers
{
    public static class UtfExtensions
    {
        public static  object GetRowValue(this UTF utf, string Name)
        {
            object Temp = utf.GetRow(Name).GetValue();

            if (Temp is ulong)
            {
                return (ulong)Temp;
            }
            if (Temp is uint)
            {
                return (uint)Temp;
            }
            if (Temp is ushort)
            {
                return (ushort)Temp;
            }
            return 0;
        }                
                
        public static long GetRowPostion(this UTF utf, string Name)
        {
            return utf.GetRow(Name)?.Position ?? -1;
        }                

        public static Row GetRow(this UTF utf, string Name)
        {
            try
            {
                //Fix iteration issue.   Only pull first row record right now.
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
