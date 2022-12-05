using MemoryPack;
using System.ComponentModel.DataAnnotations;

namespace CoreHome.Infrastructure.Models
{
    [MemoryPackable]
    public partial class FooterLink
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }
    }
}
