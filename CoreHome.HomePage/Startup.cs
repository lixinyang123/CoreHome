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
            services.AddSingleton(new ProfileService("Profile.json", new Profile()
            {
                Name = "LLLXY",
                Avatar = "https://corehome.oss-accelerate.aliyuncs.com/images/avatar.jpg",
                Info = ".Net Developer",
                QQ = "837685961",
                Email = "lixinyangemil@outlook.com",
                ICP = "豫ICP备18041216号-2",
                AdminPassword = "123456"
            }));
            services.AddSingleton(new HomePageService("Project.json", new List<Project>()));
            services.AddSingleton(new ThemeService("Theme.json", new Theme()
            {
                ThemeType = ThemeType.Auto,
                BackgroundType = BackgroundType.Color,
                MusicUrl = null
            }));
            services.AddSingleton(new NotifyService(Configuration.GetValue<string>("ServerChanSckey")));
            services.AddSingleton(new OssService(Configuration.GetSection("OssConfig").Get<OssConfig>()));

            services.Configure<CookieOptions>(config =>
            {
                config.SameSite = SameSiteMode.Strict;
            });
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

            //Linux使用Nginx反向代理，边缘服务器不启用https
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
