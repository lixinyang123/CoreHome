using coreHome.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace coreHome.DatabaseOperator
{
    public class MysqlDbConfigurator
    {
        private readonly string articleConnection;

        public MysqlDbConfigurator(IOptions<ConnectionStrings> seeting)
        {
            articleConnection = seeting.Value.ArticleConnection;
        }

        public ArticleDbContext CreateArticleDbContext()
        {
            var optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionBuilder.UseMySQL(articleConnection);
            var context = new ArticleDbContext(optionBuilder.Options);
            context.Database.EnsureCreatedAsync();
            return context;
        }

    }
}
