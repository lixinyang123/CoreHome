using CoreHome.HomePage.ViewModels;
using CoreHome.Infrastructure.Models;
using System.Collections.Generic;

namespace CoreHome.HomePage.Services
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
