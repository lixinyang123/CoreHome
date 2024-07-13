using CoreHome.Admin.Services;
using CoreHome.Data.DatabaseContext;
using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.Admin
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // 将服务添加到容器
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddDataProtection()
                .SetApplicationName("CoreHome")
                .PersistKeysToFileSystem(new(Path.Combine(StaticConfig.STORAGE_FOLDER, "/DataProtection")));

            _ = services.Configure<CookieOptions>(config => config.SameSite = SameSiteMode.Lax);

            _ = services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            //数据库上下文
            _ = services.AddDbContext<ArticleDbContext>(options =>
            {
                MySqlServerVersion version = new(new Version(8, 3, 0));

                _ = options.UseMySql(Configuration.GetConnectionString("CoreHome"), version, option =>
                {
                    _ = option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    _ = option.EnableRetryOnFailure();
                });
            });

            //Bing壁纸服务
            _ = services.AddSingleton<BingWallpaperService>();

            //个人信息服务
            _ = services.AddSingleton(new ProfileService("Profile", new Profile()));

            //项目管理服务
            _ = services.AddSingleton(new HomePageService("Project", []));

            //主题服务
            _ = services.AddSingleton(new ThemeService("Theme", new Theme()));

            //通知服务
            _ = services.AddSingleton(new NotifyService(Configuration.GetSection("PusherConfig").Get<PusherConfig>()));

            //阿里云OSS服务
            _ = services.AddSingleton(new OssService(Configuration.GetSection("OssConfig").Get<OssConfig>()));

            //安全服务
            _ = services.AddSingleton(new SecurityService("Key", new Models.Secret()
            {
                IV = Guid.NewGuid().ToString().Replace("-", "")[..16],
                Key = Guid.NewGuid().ToString().Replace("-", "")
            }));
        }

        // 配置应用服务
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error");
                _ = app.UseHsts();
            }

            _ = app.UsePathBase(new PathString("/Admin"));
            _ = app.UseWebSockets();
            _ = app.UseStaticFiles();
            _ = app.UseRouting();

            _ = app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
