using Infrastructure.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Infrastructure.common
{
    public static class BgmManager
    {
        private static readonly string configPath = @"C:/Server/coreHome/";
        private static readonly string bgmPath = @"C:/Server/coreHome/Bgm/";

        private static readonly string configFile = configPath + "Bgm.json";

        public static Bgm Bgm
        {
            get
            {
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }

                if (File.Exists(configFile))
                {
                    string content = File.ReadAllText(configFile);
                    return JsonSerializer.Deserialize<Bgm>(content);
                }
                else
                {
                    Bgm bgm = new Bgm();
                    File.WriteAllText(configFile, JsonSerializer.Serialize(bgm));
                    return bgm;
                }
            }
            set
            {
                string content = JsonSerializer.Serialize(value);
                File.WriteAllText(configFile, content);
            }
        }

        public static List<string> GetBgmList()
        {
            if (!Directory.Exists(bgmPath))
            {
                Directory.CreateDirectory(bgmPath);
            }

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
