using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace CoreHome.ReverseProxy
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // 配置服务
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddDataProtection();

            _ = services.AddRateLimiter(options =>
            {
                _ = options.AddConcurrencyLimiter("MyPolicy", option =>
                {
                    option.QueueLimit = 10;
                    option.PermitLimit = 10;
                    option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
            });

            _ = services.AddReverseProxy().LoadFromConfig(Configuration.GetSection("ReverseProxy"));
        }

        // 配置HTTP请求管道
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }

            // 速率限制
            _ = app.UseRateLimiter();

            _ = app.UseRouting();

            _ = app.UseEndpoints(endpoints => endpoints.MapReverseProxy());
        }
    }
}
