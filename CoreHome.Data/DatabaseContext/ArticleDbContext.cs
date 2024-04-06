using CoreHome.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.Data.DatabaseContext
{
    public class ArticleDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<Year>()
                .HasKey(i => i.Value);

            _ = modelBuilder.Entity<Year>()
                .Property(i => i.RowVersion)
                .IsRowVersion();

            _ = modelBuilder.Entity<Month>()
                .HasOne(i => i.Year)
                .WithMany(i => i.Months)
                .HasForeignKey(i => i.YearId);

            _ = modelBuilder.Entity<Month>()
                .Property(i => i.RowVersion)
                .IsRowVersion();

            _ = modelBuilder.Entity<Article>()
                .HasOne(i => i.Month)
                .WithMany(i => i.Articles)
                .HasForeignKey(i => i.MonthId);

            _ = modelBuilder.Entity<Article>()
                .HasOne(i => i.Category)
                .WithMany(i => i.Articles)
                .HasForeignKey(i => i.CategoryId);

            _ = modelBuilder.Entity<Article>()
                .Property(i => i.RowVersion)
                .IsRowVersion();

            _ = modelBuilder.Entity<ArticleTag>()
                .HasKey(i => new { i.ArticleId, i.TagId });

            _ = modelBuilder.Entity<ArticleTag>()
                .HasOne(i => i.Article)
                .WithMany(i => i.ArticleTags)
                .HasForeignKey(i => i.ArticleId);

            _ = modelBuilder.Entity<ArticleTag>()
                .HasOne(i => i.Tag)
                .WithMany(i => i.ArticleTags)
                .HasForeignKey(i => i.TagId);

            _ = modelBuilder.Entity<ArticleTag>()
                .Property(i => i.RowVersion)
                .IsRowVersion();

            _ = modelBuilder.Entity<Comment>()
                .HasOne(i => i.Article)
                .WithMany(i => i.Comments)
                .HasForeignKey(i => i.ArticleId);

            _ = modelBuilder.Entity<Notification>()
                .HasKey(i => i.Id);

            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    var timestampProperty = entityType.FindProperty("RowVersion");

            //    if (timestampProperty != null)
            //    {
            //        modelBuilder.Entity(entityType.ClrType).Property("RowVersion").IsRowVersion();
            //    }
            //}
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Year> Years { get; set; }

        public DbSet<Month> Months { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ArticleTag> ArticleTags { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
