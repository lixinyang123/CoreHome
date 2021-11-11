using System.ComponentModel.DataAnnotations;

namespace CoreHome.HomePage.ViewModels
{
    public class FeedbackViewModel
    {
        /// <summary>
        /// 联系方式
        /// </summary>
        [Required]
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        /// <summary>
        /// 反馈标题
        /// </summary>
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        /// <summary>
        /// 反馈验证码
        /// </summary>
        [Required]
        [Display(Name = "VerificationCode")]
        public string VerificationCode { get; set; }
    }
}
