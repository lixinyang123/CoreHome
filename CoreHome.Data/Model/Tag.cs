using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Model
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string TagName { get; set; }

        public List<ArticleTag> ArticleTags { get; set; }
    }
}
