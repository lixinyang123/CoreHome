using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class HomePageService : StaticConfig<List<Project>>
    {
        public HomePageService(string fileName, List<Project> initProjects) : base(fileName, initProjects) { }
    }
}
