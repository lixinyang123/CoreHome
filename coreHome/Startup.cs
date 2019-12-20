using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataContext.DbOperator;
using DataContext.Models;
using DataContext.CacheOperator;
using DataContext.DbConfig;

namespace coreHome
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

            services.AddSession();
            services.AddControllersWithViews();

            //数据库服务
            services.AddSingleton<IDbOperator<Comment>, CommentDbOperator>();
            services.AddSingleton<IDbOperator<Article>, ArticleDbOperator>();
            services.AddSingleton<IDbOperator<Tag>, TagDbOperator>();

            //缓存服务
            services.AddSingleton<ICacheOperator<Article>, ArticleCacheOperator>();
        }

        //配置HTTP请求
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

            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
