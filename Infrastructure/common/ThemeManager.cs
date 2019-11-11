using System;
using System.IO;
using System.Text.Json;
using Infrastructure.Models;

namespace Infrastructure.common
{
    public class ThemeManager
    {
        private readonly static string configPath = @"C:/Server/coreHome/";
        private readonly static string configFile = configPath + "theme.json";
        public Theme MyTheme {get;set;}

        public ThemeManager()
        {
            if(!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);
            
            if(File.Exists(configFile))
            {
                var jsonStr = File.ReadAllText(configFile);
                MyTheme = JsonSerializer.Deserialize<Theme>(jsonStr);
            }
            else
            {
                ResetTheme();
            }
        }

        public void ChangeTheme(Theme theme)
        {
            string content = JsonSerializer.Serialize(theme);
            SaveTheme(content);
        }

        public void ResetTheme()
        {
            MyTheme = new Theme();
            string content = JsonSerializer.Serialize(MyTheme);
            SaveTheme(content);
        }

        private void SaveTheme(string content)
        {
            File.WriteAllText(configFile,content);
        }

    }
}