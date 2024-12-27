using System.ComponentModel.DataAnnotations;

namespace CoreHome.Admin.ViewModels
{
    public class PreSearchViewModel(Guid articleCode, string title, string overview)
    {
        /// <summary>
        /// 文章识别码
        /// </summary>
        [Required]
        public Guid ArticleCode { get; } = articleCode;

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; } = title;

        /// <summary>
        /// 文章概述（限制长度为60）
        /// </summary>
        public string Overview { get; } = overview.Length > 60 ? overview[..60] + "......" : overview;
    }
}
