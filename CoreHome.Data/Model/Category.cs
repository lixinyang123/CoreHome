using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Model
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string CategoriesName { get; set; }

        public List<Article> Articles { get; set; }
    }
}
