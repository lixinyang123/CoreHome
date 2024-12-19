using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class HomePageService(string fileName, List<Project> initProjects) : StaticConfig<List<Project>>(fileName, initProjects)
    {
    }
}
