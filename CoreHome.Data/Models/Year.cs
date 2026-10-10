using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Models
{
    public class Year
    {
        [Required]
        public int Value { get; set; }

        public List<Month> Months
        {
            get => field.OrderBy(i => i.Value).ToList();
            set;
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
