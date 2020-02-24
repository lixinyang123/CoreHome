using System;
using System.Collections.Generic;

namespace CoreHome.Data.Model
{
    public class Article
    {
        public int Id { get; set; }

        public string ArticleCode { get; set; }

        public string Title { get; set; }

        public DateTime Time { get; set; }

        public int CategoriesId { get; set; }

        public Categories Categories { get; set; }

        public List<ArticleTag> ArticleTags { get; set; }

        public string Overview { get; set; }

        public string CoverUrl { get; set; }

        public string Content { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
