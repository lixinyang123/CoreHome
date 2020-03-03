using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Model
{
    public class Year
    {
        [Required]
        public int Value { get; set; }

        public List<Month> Months { get; set; }
    }
}
