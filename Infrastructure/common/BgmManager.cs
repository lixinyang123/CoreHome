using System;
using System.IO;
using System.Collections.Generic;

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
    }
}
