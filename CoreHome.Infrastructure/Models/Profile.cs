using MemoryPack;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    [MemoryPackable]
    public partial class Profile
    {
        public Profile()
        {
            WhatsNew = new List<FooterLink>()
            {
                new FooterLink()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "CoreHome",
                    Link = "https://github.com/lixinyang123/CoreHome"
                },
                new FooterLink()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "CloudShell",
                    Link = "https://www.conchbrain.club/#cloudshell"
                }
            };

            FriendLinks = new List<FooterLink>()
            {
                new FooterLink()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "LLLXY",
                    Link = "https://www.lllxy.net/"
                },
                new FooterLink()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "ConchBrainClub",
                    Link = "https://conchbrain.club/"
                }
            };

            About = new List<FooterLink>()
            {
                new FooterLink()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Github",
                    Link = "https://github.com/"
                },
                new FooterLink()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "微博",
                    Link = "https://weibo.com/"
                }
            };

            Others = new List<FooterLink>()
            {
                new FooterLink()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Privacy&Cookie",
                    Link = "/Home/Privacy"
                },
                new FooterLink()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Admin",
                    Link = "/Admin"
                }
            };
        }

        [Required]
        [Display(Name = "User Name")]
        public string Name { get; set; } = "LLLXY";

        [Required]
        [Display(Name = "User Info (Use the '#' separation)")]
        public string Info { get; set; } = ".NET Developer";

        [Required]
        [RegularExpression("[0-9]+")]
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
        [RegularExpression("[!@#$%^&*.,0-9a-zA-Z]+")]
        public string AdminPassword { get; set; }

        public List<FooterLink> WhatsNew { get; set; }

        public List<FooterLink> FriendLinks { get; set; }

        public List<FooterLink> About { get; set; }

        public List<FooterLink> Others { get; set; }

    }
}
