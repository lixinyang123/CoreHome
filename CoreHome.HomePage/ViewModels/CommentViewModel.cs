using System.ComponentModel.DataAnnotations;

namespace CoreHome.HomePage.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public Guid ArticleCode { get; set; }

        [Required]
        public string Detail { get; set; }

        [Required]
        public string VerificationCode { get; set; }
    }
}
