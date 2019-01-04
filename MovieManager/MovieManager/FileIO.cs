using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    class FileIO
    {
        public static FileInfo Replace_Extension(FileInfo Name, string Extension)
        {
            string[] temp = Name.FullName.Split('.');
            string result = "";
            for (int i = 0; i < temp.Length - 1; i++) result += temp[i] + ".";
            result += Extension;
            return new FileInfo(result);
        }
        public static FileInfo Replace_Directory(FileInfo Item, DirectoryInfo NewDirectory)
        {
            string temp = NewDirectory.FullName + Item.Name;
            return new FileInfo(temp);
        }
        public static List<FileInfo> DirSearch(DirectoryInfo Dir)
        {
            return DirSearch(Dir.FullName);
        }
        private static List<FileInfo> DirSearch(string sDir, List<FileInfo> temp = null)
        {
            if (temp == null) temp = new List<FileInfo>();
            DirectoryInfo di = new DirectoryInfo(sDir);

            foreach (var s in Directory.GetDirectories(sDir)) DirSearch(s, temp);
            foreach (var item in di.GetFiles()) temp.Add(item);
            return temp;
        }

        public static void CreateFile(FileInfo item)
        {
            FileStream fileStream = item.Create();
            fileStream.Close();
            item.Refresh();
        }
    }
}
