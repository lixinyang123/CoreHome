using CoreHome.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CoreHome.Admin
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            Initialize(host).Run();
        }

        private static IHost Initialize(IHost host)
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            ArticleDbContext dbContext = serviceScope.ServiceProvider.GetService<ArticleDbContext>();
            dbContext.Database.Migrate();
            return host;
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
