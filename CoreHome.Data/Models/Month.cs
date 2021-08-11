using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Models
{
    public class Month
    {
        public int Id { get; set; }

        [Required]
        public int Value { get; set; }

        public int YearId { get; set; }

        [Required]
        public Year Year { get; set; }

        public List<Article> Articles { get; set; }
    }
}
