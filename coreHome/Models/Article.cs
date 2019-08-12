using System.ComponentModel.DataAnnotations;

namespace coreHome.Models
{
    public class Article
    {
        /// <summary>
        /// 博客ID
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 概述
        /// </summary>
        public string Overview { get; set; }
    }
}
