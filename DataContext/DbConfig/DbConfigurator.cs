using DataContext.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DataContext.DbConfig
{
    public class DbConfigurator
    {
        private const string articleConnection = "server=localhost;user id=root;password=lxy15937905153;database=articles";
        private const string redisConnection = "localhost";

        private static readonly object dbLock = new object();
        private static ArticleDbContext context = null;

        private DbConfigurator() { }

        ~DbConfigurator()
        {
            context.Dispose();
        }

        public static ArticleDbContext CreateArticleDbContext()
        {
            lock (dbLock)
            {
                if (context == null)
                {
                    DbContextOptionsBuilder<ArticleDbContext> optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
                    optionBuilder.UseMySQL(articleConnection);
                    context = new ArticleDbContext(optionBuilder.Options);
                    context.Database.EnsureCreatedAsync();
                }
                return context;
            }
        }

        public static ConnectionMultiplexer CreateCacheContext()
        {
            return ConnectionMultiplexer.Connect(redisConnection);
        }

    }
}
