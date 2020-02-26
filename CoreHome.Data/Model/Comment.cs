using System;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Model
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public string Detail { get; set; }

        [Required]
        public int ArticleId { get; set; }

        public Article Article { get; set; }

    }
}
