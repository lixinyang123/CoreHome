using System.ComponentModel.DataAnnotations;

namespace CoreHome.Data.Models
{
    public class Year
    {
        [Required]
        public int Value { get; set; }

        private List<Month> months;

        public List<Month> Months
        {
            get => months.OrderBy(i => i.Value).ToList();
            set => months = value;
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
