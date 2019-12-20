using DataContext.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DataContext.DbConfig
{
    public class DbConfigurator
    {
        private static string DbConnectionString { get; set; }
        private static string RedisConnectionStrin { get; set; }

        private static ArticleDbContext dbContext = null;
        private static ConnectionMultiplexer cacheContext = null;

        private DbConfigurator() { }

        ~DbConfigurator()
        {
            dbContext.Dispose();
            cacheContext.Dispose();
        }

        public static void Init(string dbConnectionStr,string redisConnectionStr)
        {
            DbConnectionString = dbConnectionStr;
            RedisConnectionStrin = redisConnectionStr;
            InitDbContext();
            InitCacheContext();
        }

        private static void InitDbContext()
        {
            DbContextOptionsBuilder<ArticleDbContext> optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionBuilder.UseMySQL(DbConnectionString);
            dbContext = new ArticleDbContext(optionBuilder.Options);
            dbContext.Database.EnsureCreatedAsync();
        }

        private static void InitCacheContext()
        {
            cacheContext = ConnectionMultiplexer.Connect(RedisConnectionStrin);
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
