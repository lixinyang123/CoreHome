using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class ThemeService : StaticConfig<Theme>
    {
        public ThemeService() : base("Theme.json", new Theme()
        {
            ThemeType = ThemeType.Auto,
            BackgroundType = BackgroundType.Color,
            MusicUrl = null
        })
        { }
    }
}
