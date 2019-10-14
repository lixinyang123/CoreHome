using DataContext.ModelDbContext;
using Microsoft.EntityFrameworkCore;

namespace DataContext.DbConfig
{
    public class DbConfigurator
    {
        private readonly string articleConnection;

        public DbConfigurator()
        {
            articleConnection = "server=localhost;user id=root;password=lxy15937905153;database=articles";
        }

        public ArticleDbContext CreateArticleDbContext()
        {
            DbContextOptionsBuilder<ArticleDbContext> optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionBuilder.UseMySQL(articleConnection);
            ArticleDbContext context = new ArticleDbContext(optionBuilder.Options);
            context.Database.EnsureCreatedAsync();
            return context;
        }

    }
}
