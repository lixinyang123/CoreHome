using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public enum ThemeType { Auto, White, Black }
    public enum BackgroundType { Color, Bing, Custom }

    public class Theme
    {
        public ThemeType ThemeType { get; set; } = ThemeType.Auto;

        public BackgroundType BackgroundType { get; set; } = BackgroundType.Color;

        [Url]
        public string MusicUrl { get; set; } = string.Empty;
    }
}