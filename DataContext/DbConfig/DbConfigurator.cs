using DataContext.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DataContext.DbConfig
{
    public class DbConfigurator
    {
        private readonly string articleConnection;
        private readonly string redisConnection;

        public DbConfigurator()
        {
            articleConnection = "server=localhost;user id=root;password=lxy15937905153;database=articles";
            redisConnection = "localhost,DefaultDatabase=0";
        }

        public ArticleDbContext CreateArticleDbContext()
        {
            DbContextOptionsBuilder<ArticleDbContext> optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionBuilder.UseMySQL(articleConnection);
            ArticleDbContext context = new ArticleDbContext(optionBuilder.Options);
            context.Database.EnsureCreatedAsync();
            return context;
        }

        public IDatabase CreateArticleCacheContext()
        {
            using ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnection);
            return redis.GetDatabase();
        }

    }
}
