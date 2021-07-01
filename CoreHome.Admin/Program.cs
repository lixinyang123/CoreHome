using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreHome.Admin
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using IServiceScope serviceScope = host.Services.CreateScope();
            serviceScope.ServiceProvider.GetService<ArticleDbContext>().Database.EnsureCreated();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
