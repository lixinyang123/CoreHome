using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    public class Profile
    {
        public Profile()
        {
            WhatsNew = new List<FooterLink>();
            FriendLinks = new List<FooterLink>();
            About = new List<FooterLink>();
        }

        [Required]
        [Display(Name = "User Name")]
        public string Name { get; set; } = "LLLXY";

        [Required]
        [Url]
        [Display(Name = "User Avatar")]
        public string Avatar { get; set; } = "https://corehome.oss-accelerate.aliyuncs.com/images/avatar.jpg";

        [Required]
        [Display(Name = "User Info (eg: .Net Developer)")]
        public string Info { get; set; } = ".Net Developer";

        [Required]
        [Display(Name = "QQ")]
        public string QQ { get; set; } = "837685961";

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "lixinyangemil@outlook.com";

        [Required]
        [Display(Name = "ICP")]
        public string ICP { get; set; } = "豫ICP备12345678号";

        [Required]
        [MinLength(8)]
        [Display(Name = "Admin Password")]
        public string AdminPassword { get; set; }

        public List<FooterLink> WhatsNew { get; set; }

        public List<FooterLink> FriendLinks { get; set; }

        public List<FooterLink> About { get; set; }

    }
}
