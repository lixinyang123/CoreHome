using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class ThemeService : StaticConfig<Theme>
    {
        public ThemeService(string fileName, Theme initTheme) : base(fileName, initTheme) { }
    }
}
