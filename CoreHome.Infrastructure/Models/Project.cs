using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public enum ProjectSize { Big, Middle, Small }

    public class Project
    {
        [Required]
        public ProjectSize Size { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Detail { get; set; }

        [Required]
        public string CoverUrl { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public Dictionary<string, string> Tips { get; set; }
    }
}
