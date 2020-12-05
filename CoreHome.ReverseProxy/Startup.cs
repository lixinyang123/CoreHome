using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddReverseProxy().LoadFromConfig(Configuration.GetSection("ReverseProxy")); 
        }

        // 配置HTTP请求管道
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapReverseProxy();
            });
        }
    }
}
