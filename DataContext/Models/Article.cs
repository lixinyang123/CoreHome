using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    /// <summary>
    /// 博客文章
    /// </summary>
    public class Article
    {
        public Article()
        {
            Comments = new List<Comment>();
            Tags = new List<Tag>();
        }

        /// <summary>
        /// 编号ID
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 博客ID
        /// </summary>
        public string ArticleID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary>
        public List<Tag> Tags{ get; set; }

        /// <summary>
        /// 概述
        /// </summary>
        public string Overview { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 博客内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 博客评论
        /// </summary>
        public List<Comment> Comments { get; set; }
    }

}
