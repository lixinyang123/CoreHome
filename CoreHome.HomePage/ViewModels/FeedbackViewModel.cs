using System.ComponentModel.DataAnnotations;

namespace CoreHome.HomePage.ViewModels
{
    public class FeedbackViewModel
    {
        [Required]
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "VerificationCode")]
        public string VerificationCode { get; set; }
    }
}
