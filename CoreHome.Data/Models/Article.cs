using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Models
{
    public class Article
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 博客唯一识别码
        /// </summary>
        [Required]
        public Guid ArticleCode { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime Time { get; set; }

        public int MonthId { get; set; }

        /// <summary>
        /// 所属月份
        /// </summary>
        [Required]
        public Month Month { get; set; }

        public int CategoryId { get; set; }

        /// <summary>
        /// 所属类别
        /// </summary>
        [Required]
        public Category Category { get; set; }

        /// <summary>
        /// 博客标签中间表
        /// </summary>
        public List<ArticleTag> ArticleTags { get; set; }

        /// <summary>
        /// 概述
        /// </summary>
        [Required]
        public string Overview { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public List<Comment> Comments { get; set; }
    }
}
