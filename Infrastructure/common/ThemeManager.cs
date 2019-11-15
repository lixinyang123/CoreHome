using Infrastructure.Models;
using System.IO;
using System.Text.Json;

namespace Infrastructure.common
{
    public static class ThemeManager
    {
        private static readonly string configPath = @"C:/Server/coreHome/";
        private static readonly string configFile = configPath + "theme.json";

        public static readonly string backgroundUrl = @"C:/Server/coreHome/Background.jpg";

        public static Theme GetTheme()
        {
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            if (File.Exists(configFile))
            {
                string jsonStr = File.ReadAllText(configFile);
                return JsonSerializer.Deserialize<Theme>(jsonStr);
            }
            else
            {
                ResetTheme();
                return new Theme();
            }
        }

        public static void ChangeTheme(Theme theme)
        {
            string content = JsonSerializer.Serialize(theme);
            SaveTheme(content);
        }

        public static void ResetTheme()
        {
            string content = JsonSerializer.Serialize(new Theme());
            SaveTheme(content);
        }

        private static void SaveTheme(string content)
        {
            File.WriteAllText(configFile, content);
        }

    }
}