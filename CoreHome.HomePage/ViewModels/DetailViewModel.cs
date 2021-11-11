using CoreHome.Data.Models;

namespace CoreHome.HomePage.ViewModels
{
    public class DetailViewModel
    {
        /// <summary>
        /// 文章
        /// </summary>
        public Article Article { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public CommentViewModel CommentViewModel { get; set; }
    }
}
