using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.RateLimiting;
using System.Runtime.InteropServices;
using System.Threading.RateLimiting;

namespace CoreHome.ReverseProxy
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // 配置服务
        public void ConfigureServices(IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _ = services.AddDataProtection().SetApplicationName("CoreHome")
                    .PersistKeysToFileSystem(new DirectoryInfo(@"C:/Server/CoreHome/"));
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _ = services.AddDataProtection().SetApplicationName("CoreHome")
                    .PersistKeysToFileSystem(new DirectoryInfo(@"/home/Server/CoreHome/"));
            }

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
            _ = app.UseRateLimiter(
                new RateLimiterOptions()
                {
                    OnRejected = (context, cancellationToken) =>
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        return new ValueTask();
                    }
                }.AddConcurrencyLimiter("MyPolicy", option =>
                {
                    option.QueueLimit = 10;
                    option.PermitLimit = 10;
                    option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                })
            );

            _ = app.UseRouting();

            _ = app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapReverseProxy();
            });
        }
    }
}
