using DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DataContext.ModelDbContext
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class ArticleDbContext : DbContext
    {
        public ArticleDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Article> Article { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<Tag> Tag { get; set; }
    }
}
