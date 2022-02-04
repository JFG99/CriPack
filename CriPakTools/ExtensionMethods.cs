using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriPakTools
{
    public static class ExtensionMethods
    {
        public static string GetSafePath(this string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

        public static Dictionary<string, string> ReadBatchScript(this string batch_script_name)
        {
            //---------------------
            // TXT内部
            // original_file_name(in cpk),patch_file_name(in folder)
            // /HD_font_a.ftx,patch/BOOT.cpk_unpacked/HD_font_a.ftx
            // OTHER/ICON0.PNG,patch/BOOT.cpk_unpacked/OTHER/ICON0.PNG

            Dictionary<string, string> flist = new Dictionary<string, string>();

            StreamReader sr = new StreamReader(batch_script_name, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.IndexOf(",") > -1)
                //只读取格式正确的行
                {
                    line = line.Replace("\n", "");
                    line = line.Replace("\r", "");
                    string[] currentValue = line.Split(',');
                    flist.Add(currentValue[0], currentValue[1]);
                }


            }
            sr.Close();

            return flist;
        }
    }
}
