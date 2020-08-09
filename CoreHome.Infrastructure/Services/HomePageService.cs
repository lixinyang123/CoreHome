using CoreHome.Infrastructure.Models;
using System.Collections.Generic;

namespace CoreHome.Infrastructure.Services
{
    public class HomePageService : StaticConfig<List<Project>>
    {
        public HomePageService(string fileName, List<Project> initProjects) : base(fileName, initProjects) { }
    }
}
