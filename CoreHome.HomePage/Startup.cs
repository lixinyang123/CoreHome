using CoreHome.Data.DatabaseContext;
using CoreHome.Infrastructure.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CoreHome.HomePage
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // 将服务添加到容器
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

            _ = services.Configure<CookieOptions>(config => config.SameSite = SameSiteMode.Lax);

            _ = services.AddSession();

            _ = services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            //数据库上下文
            _ = services.AddDbContext<ArticleDbContext>(options =>
            {
                MySqlServerVersion version = new(new Version(8, 3, 0));

                _ = options.UseMySql(Configuration.GetConnectionString("CoreHome"), version, option =>
                {
                    _ = option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    _ = option.EnableStringComparisonTranslations();
                    _ = option.EnableRetryOnFailure();
                });
            });

            //验证服务
            _ = services.AddScoped<VerificationCodeService>();

            //Bing壁纸服务
            _ = services.AddSingleton<BingWallpaperService>();

            //搜索引擎服务
            _ = services.AddSingleton(new SearchEngineService(Configuration.GetValue<string>("BaiduLinkSubmit")));

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
        }

        //配置HTTP请求
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

            _ = app.UseSession();
            _ = app.UseStaticFiles();
            _ = app.UseRouting();
            _ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{para?}");
            });

        }
    }
}
