using Microsoft.EntityFrameworkCore;

namespace coreHome.Models
{
    public class ArticleDbContext : DbContext
    {
        public ArticleDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Article> Article { get; set; }
    }
}
