using CriPakInterfaces;
using CriPakInterfaces.Models.Components;
using System.Collections.Generic;
using System.Text;

namespace CriPakRepository.Helpers
{
    public static class ExtensionMethods
    {
        //CriFile method?
        public static bool CheckListRedundant(this List<CriFile> input)
        {

            bool result = false;
            List<string> tmp = new List<string>();
            for (int i = 0; i < input.Count; i++)
            {
                string name = ((input[i].DirName != null) ?
                                        input[i].DirName + "/" : "") + input[i].FileName;
                if (!tmp.Contains(name))
                {
                    tmp.Add(name);
                }
                else
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }

        //method on Endian Reader?
        public static string ReadCString(this IEndianReader br, int MaxLength = -1, long lOffset = -1, Encoding enc = null)
        {
            int Max;
            if (MaxLength == -1)
                Max = 255;
            else
                Max = MaxLength;

            long fTemp = br.BaseStream.Position;
            byte bTemp = 0;
            int i = 0;
            string result = "";

            if (lOffset > -1)
            {
                br.BaseStream.Seek(lOffset, SeekOrigin.Begin);
            }

            do
            {
                bTemp = br.ReadByte();
                if (bTemp == 0)
                    break;
                i += 1;
            } while (i < Max);

            if (MaxLength == -1)
                Max = i + 1;
            else
                Max = MaxLength;

            if (lOffset > -1)
            {
                br.BaseStream.Seek(lOffset, SeekOrigin.Begin);

                if (enc == null)
                    result = Encoding.UTF8.GetString(br.ReadBytes(i));
                else
                    result = enc.GetString(br.ReadBytes(i));

                br.BaseStream.Seek(fTemp, SeekOrigin.Begin);
            }
            else
            {
                br.BaseStream.Seek(fTemp, SeekOrigin.Begin);
                if (enc == null)
                    result = Encoding.ASCII.GetString(br.ReadBytes(i));
                else
                    result = enc.GetString(br.ReadBytes(i));

                br.BaseStream.Seek(fTemp + Max, SeekOrigin.Begin);
            }

            return result;
        }

        public static byte[] GetData(this IEndianReader br, long offset, int size)
        {
            byte[] result = null;
            long backup = br.BaseStream.Position;
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            result = br.ReadBytes(size);
            br.BaseStream.Seek(backup, SeekOrigin.Begin);
            return result;
        }

        public static string GetSafePath(this string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
