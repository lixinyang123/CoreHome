using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Model
{
    public class Month
    {
        [Required]
        public int Value { get; set; }

        public int YearId { get; set; }

        [Required]
        public Year Year { get; set; }

        public List<Article> Articles { get; set; }
    }
}
