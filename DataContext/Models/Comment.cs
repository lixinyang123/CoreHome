namespace DataContext.Models
{
    public class Comment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 博客ID
        /// </summary>
        public int ArticleID { get; set; }
    }
}
