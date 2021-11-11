using System.ComponentModel.DataAnnotations;

namespace CoreHome.HomePage.ViewModels
{
    public class PreSearchViewModel
    {
        public PreSearchViewModel(Guid articleCode, string title, string overview)
        {
            ArticleCode = articleCode;
            Title = title;
            Overview = overview[..60] + "......";
        }

        /// <summary>
        /// 文章识别码
        /// </summary>
        [Required]
        public Guid ArticleCode { get; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 文章概述（限制长度为60）
        /// </summary>
        public string Overview { get; }
    }
}
