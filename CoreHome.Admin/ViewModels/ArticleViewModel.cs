using System.ComponentModel.DataAnnotations;

namespace CoreHome.Admin.ViewModels
{
    public class ArticleViewModel
    {
        public Guid ArticleCode { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Tag")]
        public string TagStr { get; set; }

        [Required]
        [Display(Name = "Overview")]
        public string Overview { get; set; }

        [Url]
        [Display(Name = "CoverUrl")]
        public string CoverUrl { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

    }
}
