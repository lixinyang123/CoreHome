namespace Infrastructure.Models
{
    public enum ThemeType { Auto, White, Black }
    public enum BackgroundType { Color, Image, Video }

    public class Theme
    {
        public ThemeType themeType { get; set; } = ThemeType.Auto;
        public BackgroundType backgroundType { get; set; } = BackgroundType.Color;
    }
}