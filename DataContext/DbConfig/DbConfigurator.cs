using DataContext.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DataContext.DbConfig
{
    public static class DbConfigurator
    {
        private static string DbConnectionString { get; set; }
        private static string RedisConnectionStrin { get; set; }

        public static void Init(string dbConnectionStr, string redisConnectionStr)
        {
            DbConnectionString = dbConnectionStr;
            RedisConnectionStrin = redisConnectionStr;
            GetDbContext().Database.EnsureCreatedAsync();
        }

        public static ArticleDbContext GetDbContext()
        {
            DbContextOptionsBuilder<ArticleDbContext> optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionBuilder.UseMySQL(DbConnectionString);
            ArticleDbContext dbContext = new ArticleDbContext(optionBuilder.Options);
            return dbContext;
        }

        public static ConnectionMultiplexer GetCacheContext()
        {
            ConnectionMultiplexer cacheContext = ConnectionMultiplexer.Connect(RedisConnectionStrin);
            return cacheContext;
        }

    }
}
