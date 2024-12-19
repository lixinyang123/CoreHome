using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class ThemeService(string fileName, Theme initTheme) : StaticConfig<Theme>(fileName, initTheme)
    {
    }
}
