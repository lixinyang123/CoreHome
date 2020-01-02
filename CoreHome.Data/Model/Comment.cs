using System;

namespace CoreHome.Data.Model
{
    public class Comment
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public string Detail { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }
    }
}
