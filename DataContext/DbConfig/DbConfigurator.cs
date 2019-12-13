using DataContext.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DataContext.DbConfig
{
    public class DbConfigurator
    {
        private readonly string articleConnection = "server=localhost;user id=root;password=lxy15937905153;database=articles";
        private readonly string redisConnection = "localhost";

        private readonly ArticleDbContext context;

        public DbConfigurator()
        {
            DbContextOptionsBuilder<ArticleDbContext> optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionBuilder.UseMySQL(articleConnection);
            context = new ArticleDbContext(optionBuilder.Options);
            context.Database.EnsureCreatedAsync();
        }

        ~DbConfigurator()
        {
            context.Dispose();
        }

        public ArticleDbContext GetArticleDbContext()
        {
            return context;
        }

        public ConnectionMultiplexer CreateCacheContext()
        {
            return ConnectionMultiplexer.Connect(redisConnection);
        }

    }
}
