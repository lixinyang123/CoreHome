using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public enum ProjectSize { Big, Middle, Small }

    public class Project
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Size")]
        public ProjectSize Size { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Detail")]
        public string Detail { get; set; }

        [Url]
        [Required]
        [Display(Name = "CoverUrl")]
        public string CoverUrl { get; set; }

        [Url]
        [Required]
        [Display(Name = "Project Href")]
        public string Link { get; set; }

        [Required]
        [Display(Name = "Tips")]
        public string Tip { get; set; }
    }
}
