namespace Infrastructure.Models
{
    public enum ThemeType{Auto,Black,White}
    public enum BackgroundType{Color,Image,Video}

    public class Theme
    {
        public ThemeType themeType{get;set;} = ThemeType.Auto;
        public BackgroundType backgroundType{get;set;} = BackgroundType.Color;
    }
}