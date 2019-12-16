using DataContext.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DataContext.DbConfig
{
    public class DbConfigurator
    {
        private const string articleConnection = "server=localhost;user id=root;password=lxy15937905153;database=articles";
        private const string redisConnection = "localhost";

        private static ArticleDbContext dbContext = null;
        private static ConnectionMultiplexer cacheContext = null;

        private DbConfigurator() { }

        ~DbConfigurator()
        {
            dbContext.Dispose();
            cacheContext.Dispose();
        }

        public static void InitConfigurator()
        {
            InitDbContext();
            InitCacheContext();
        }

        private static void InitDbContext()
        {
            DbContextOptionsBuilder<ArticleDbContext> optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionBuilder.UseMySQL(articleConnection);
            dbContext = new ArticleDbContext(optionBuilder.Options);
            dbContext.Database.EnsureCreatedAsync();
        }

        private static void InitCacheContext()
        {
            cacheContext = ConnectionMultiplexer.Connect(redisConnection);
        }

        public static ArticleDbContext DbContext
        {
            get
            {
                if (dbContext == null)
                {
                    InitDbContext();
                }
                return dbContext;
            }
        }

        public static ConnectionMultiplexer CacheContext
        {
            get
            {
                if (cacheContext == null)
                {
                    InitCacheContext();
                }
                return cacheContext;
            }
        }

    }
}
