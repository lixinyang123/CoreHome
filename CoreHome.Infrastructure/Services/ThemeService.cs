using CoreHome.Infrastructure.Models;
using System.IO;
using System.Text.Json;

namespace CoreHome.Infrastructure.Services
{
    public class ThemeService
    {
        private const string configPath = @"C:/Server/CoreHome/";
        private const string configFile = configPath + "Theme.json";

        public Theme Theme
        {
            get
            {
                return JsonSerializer.Deserialize<Theme>(File.ReadAllText(configFile));
            }
            set
            {
                File.WriteAllText(configFile, JsonSerializer.Serialize(value));
            }
        }

        public ThemeService()
        {
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            if (!File.Exists(configFile))
            {
                Theme = new Theme()
                {
                    ThemeType = ThemeType.Auto,
                    BackgroundType = BackgroundType.Color,
                    MusicUrl = null
                };
            }
        }

    }
}
