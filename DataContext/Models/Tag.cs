using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Tag
    {
        [Key]
        public int ID { get; set; }

        public string TagName { get; set; }

        public string ArticleID { get; set; }
    }
}
