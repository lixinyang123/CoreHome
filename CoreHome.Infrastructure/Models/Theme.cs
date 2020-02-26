using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public enum ThemeType { Auto, White, Black }
    public enum BackgroundType { Color,  Bing }

    public class Theme
    {
        public ThemeType ThemeType { get; set; }

        public BackgroundType BackgroundType { get; set; }

        [Url]
        public string MusicUrl { get; set; }
    }
}