using System.Collections.Generic;

namespace CoreHome.Data.Model
{
    public class Tag
    {
        public int Id { get; set; }

        public string TagName { get; set; }

        public List<ArticleTag> ArticleTags { get; set; }
    }
}
