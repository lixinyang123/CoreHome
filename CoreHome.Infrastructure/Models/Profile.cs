using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public class Profile
    {
        [Required]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        [Required]
        [Url]
        [Display(Name = "User Avatar")]
        public string Avatar { get; set; }

        [Required]
        [Display(Name = "User Info (eg: .Net Developer)")]
        public string Info { get; set; }

        [Required]
        [Display(Name = "QQ")]
        public string QQ { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "ICP")]
        public string ICP { get; set; }

        [Required]
        [MinLength(8)]
        [Display(Name = "Admin Password")]
        public string AdminPassword { get; set; }

        public List<FooterLink> WhatsNew { get; set; }

        public List<FooterLink> FriendLinks { get; set; }

        public List<FooterLink> About { get; set; }

    }
}
