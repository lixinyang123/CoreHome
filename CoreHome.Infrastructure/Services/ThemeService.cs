using CoreHome.Infrastructure.Models;
using System.IO;
using System.Text.Json;

namespace CoreHome.Infrastructure.Services
{
    public class ThemeService
    {
        private const string configPath = @"C:/Server/CoreHome/";
        private const string configFile = configPath + "Theme.json";

        private Theme theme;

        public Theme Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;
                Directory.CreateDirectory(configPath);
                File.WriteAllText(configFile, JsonSerializer.Serialize(theme));
            }
        }

        public ThemeService()
        {
            if (!File.Exists(configFile))
            {
                Theme = new Theme()
                {
                    ThemeType = ThemeType.Auto,
                    BackgroundType = BackgroundType.Color,
                    MusicUrl = null
                };
            }
            else
            {
                Theme = JsonSerializer.Deserialize<Theme>(File.ReadAllText(configFile));
            }
        }

    }
}
