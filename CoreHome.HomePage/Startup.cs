using CoreHome.Data.DatabaseContext;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

namespace CoreHome.HomePage
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

            services.AddDbContext<ArticleDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("ArticleDb"), mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                });
            });

            services.AddScoped<VerificationCodeService>();
            services.AddSingleton<BingWallpaperService>();
            services.AddSingleton<SearchEngineService>();
            services.AddSingleton<ThemeService>();
            services.AddSingleton(new NotifyService(Configuration.GetValue<string>("ServerChanSckey")));

            services.Configure<CookieOptions>(config =>
            {
                config.SameSite = SameSiteMode.Strict;
            });
        }

        //配置HTTP请求
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ArticleDbContext articleDbContext)
        {
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
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{para?}");
            });

        }
    }
}
