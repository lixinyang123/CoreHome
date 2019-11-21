using System.Collections.Generic;
using System.IO;

namespace Infrastructure.common
{
    public static class BgmManager
    {
        public static readonly string bgmPath = @"C:/Server/coreHome/Bgm/";

        public static List<string> GetBgmList()
        {
            string[] paths = Directory.GetFiles(bgmPath);
            List<string> musics = new List<string>();
            foreach (string path in paths)
            {
                FileInfo fileInfo = new FileInfo(path);
                musics.Add(fileInfo.Name);
            }
            return musics;
        }

        public static void SaveMusic(string fileName, byte[] buffer)
        {
            string fullPath = bgmPath + fileName;
            File.WriteAllBytes(fullPath, buffer);
        }

        public static void DelMusic(string fileName)
        {
            File.Delete(bgmPath + fileName);
        }

    }
}
