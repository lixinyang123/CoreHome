namespace Infrastructure.Models
{
    public enum ThemeType { Auto, White, Black }
    public enum BackgroundType { Color, Image, Bing }

    public class Theme
    {
        public ThemeType themeType = ThemeType.Auto;
        public BackgroundType backgroundType = BackgroundType.Color;
    }
}