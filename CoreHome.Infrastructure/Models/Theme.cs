namespace Infrastructure.Models
{
    public enum ThemeType { Auto, White, Black }
    public enum BackgroundType { Color, Image, Bing }

    public class Theme
    {
        public ThemeType ThemeType { get; set; } = ThemeType.Auto;
        public BackgroundType BackgroundType { get; set; } = BackgroundType.Color;
    }
}