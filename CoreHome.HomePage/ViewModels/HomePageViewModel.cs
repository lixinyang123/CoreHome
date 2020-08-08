using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.HomePage.ViewModels
{
    public class HomePageViewModel
    {
        public List<Project> Categories { get; set; }
    }

    public class Project
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Detail { get; set; }

        [Required]
        public string CoverUrl { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public Dictionary<string,string> Tips { get; set; }
    }
}
