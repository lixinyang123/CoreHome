using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public enum ProjectSize { Big, Middle, Small }

    public class Project
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "展示区域大小")]
        public ProjectSize Size { get; set; }

        [Required]
        [Display(Name = "项目名称")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "项目概述")]
        public string Detail { get; set; }

        [Url]
        [Required]
        [Display(Name = "项目封面链接")]
        public string CoverUrl { get; set; }

        [Url]
        [Required]
        [Display(Name = "项目链接")]
        public string Link { get; set; }

        [Required]
        [Display(Name = "项目提示文本")]
        public string Tip { get; set; }
    }
}
