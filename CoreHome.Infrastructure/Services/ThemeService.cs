using CoreHome.Infrastructure.Models;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace CoreHome.Infrastructure.Services
{
    public class ThemeService
    {
        private readonly string configPath;
        private readonly string configFile;

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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                configPath = @"C:/Server/CoreHome/";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                configPath = @"/home/Server/CoreHome/";
            }

            configFile = configPath + "Theme.json";

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
