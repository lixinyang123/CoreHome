using System.ComponentModel.DataAnnotations;

namespace CoreHome.HomePage.ViewModels
{
    public class CommentViewModel
    {
        /// <summary>
        /// 博客 Code
        /// </summary>
        [Required]
        public Guid ArticleCode { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 评论详情
        /// </summary>
        [Required]
        public string Detail { get; set; }

        /// <summary>
        /// 评论验证码
        /// </summary>
        [Required]
        public string VerificationCode { get; set; }
    }
}
