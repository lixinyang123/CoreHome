using admin.Attributes;
using DataContext.CacheOperator;
using DataContext.DbConfig;
using DataContext.DbOperator;
using DataContext.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 将服务添加到容器
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();

            //数据库服务
            services.AddSingleton<IDbOperator<Article>, ArticleDbOperator>();
            services.AddSingleton<IDbOperator<Comment>, CommentDbOperator>();
            services.AddSingleton<IDbOperator<Tag>, TagDbOperator>();

            //缓存服务
            services.AddSingleton<ICacheOperator<Article>, ArticleCacheOperator>();

            //注册缓存过滤器
            services.AddSingleton<DataChanged>();
        }

        // 配置应用服务
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //初始化并预热efcore
            DbConfigurator.Init(
                Configuration.GetConnectionString("DbConnectionString"),
                Configuration.GetConnectionString("RedisConnectionStrin"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseWebSockets();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            //开发环境添加Admin路由，模拟工作环境的真实路径
            if (env.IsDevelopment())
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "/Admin/{controller=Home}/{action=Index}/{id?}");
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            }
        }
    }
}
