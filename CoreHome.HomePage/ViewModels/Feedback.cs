using System.ComponentModel.DataAnnotations;

namespace CoreHome.HomePage.ViewModels
{
    public class Feedback
    {
        [Required]
        [Display(Name = "联系方式")]
        public string Contact { get; set; }

        [Required]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "问题描述")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "验证码")]
        public string VerificationCode { get; set; }
    }
}
