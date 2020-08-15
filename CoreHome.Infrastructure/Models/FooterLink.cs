using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public class FooterLink
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Url]
        [Required]
        public string Link { get; set; }
    }
}
