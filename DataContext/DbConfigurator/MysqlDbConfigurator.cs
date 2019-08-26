using DataContext.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataContext.Models;
using System.IO;

namespace DataContext.DbConfigurator
{
    public class MysqlDbConfigurator
    {
        private readonly string articleConnection;

        public MysqlDbConfigurator()
        {
            //读取配置文件
            string jsonStr = File.ReadAllText("config.json");
            //解析配置文件
            ConnectionStrings connectionStrings = JsonConvert.DeserializeObject<ConnectionStrings>(jsonStr);
            articleConnection = connectionStrings.ArticleConnection;
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
