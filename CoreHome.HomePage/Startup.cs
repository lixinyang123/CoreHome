using CoreHome.Data.DatabaseContext;
using CoreHome.Infrastructure.Models;
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
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
            services.Configure<CookieOptions>(config =>
            {
                config.SameSite = SameSiteMode.Lax;
            });

            services.AddSession();
            services.AddControllersWithViews();

            services.AddDbContext<ArticleDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("CoreHome"), mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                });
            });

            services.AddScoped<VerificationCodeService>();
            services.AddSingleton<BingWallpaperService>();
            services.AddSingleton(new SearchEngineService(Configuration.GetValue<string>("BaiduLinkSubmit")));
            services.AddSingleton(new ProfileService("Profile.json", new Profile()));
            services.AddSingleton(new HomePageService("Project.json", new List<Project>()));
            services.AddSingleton(new ThemeService("Theme.json", new Theme()));
            services.AddSingleton(new NotifyService(Configuration.GetValue<string>("ServerChanSckey")));
            services.AddSingleton(new OssService(Configuration.GetSection("OssConfig").Get<OssConfig>()));
        }

        //配置HTTP请求
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            //Linux使用Nginx反向代理，不启用https
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                app.UseHttpsRedirection();
            }

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
