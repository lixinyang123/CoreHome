using CoreHome.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.Data.DatabaseContext
{
    public class ArticleDbContext : DbContext
    {
        public ArticleDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(i => i.Article)
                .WithMany(i => i.Comments)
                .HasForeignKey(i => i.ArticleId);

            modelBuilder.Entity<ArticleTag>()
                .HasKey(i => new { i.ArticleId, i.TagId });

            modelBuilder.Entity<ArticleTag>()
                .HasOne(i => i.Article)
                .WithMany(i => i.ArticleTags)
                .HasForeignKey(i => i.ArticleId);

            modelBuilder.Entity<ArticleTag>()
                .HasOne(i => i.Tag)
                .WithMany(i => i.ArticleTags)
                .HasForeignKey(i => i.TagId);
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ArticleTag> ArticleTags { get; set; }
    }
}
