using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public class Profile
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Url]
        public string Avatar { get; set; }

        [Required]
        public string Info { get; set; }

        [Required]
        public string QQ { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ICP { get; set; }

        [Required]
        [MinLength(8)]
        public string AdminPassword { get; set; }
    }
}
