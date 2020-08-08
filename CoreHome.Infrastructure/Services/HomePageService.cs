using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.ViewModels;
using System.Collections.Generic;

namespace CoreHome.Infrastructure.Services
{
    public class HomePageService : StaticConfig<HomePageViewModel>
    {
        public HomePageService() : base("HomePage.json", new HomePageViewModel()
        {
            Categories = new List<Project>()
        })
        { }
    }
}
