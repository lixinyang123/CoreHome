using CoreHome.Admin.Attributes;
using CoreHome.Admin.Filter;
using CoreHome.Admin.Models;
using CoreHome.Admin.ViewModels;
using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class OverviewController(IConfiguration configuration, 
        ArticleDbContext articleDbContext) : Controller
    {
        private static byte[] data;

        private const int length = 1024 * 1024 * 1;

        private readonly WebSocketPusher pusher = new();

        private readonly IConfiguration configuration = configuration;

        private readonly ArticleDbContext articleDbContext = articleDbContext;

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Overview";
            OverviewViewModel viewModel = new()
            {
                StartupTime = configuration.GetValue<string>("STARTUP_TIME"),
                BlogCount = articleDbContext.Articles.Count(),
                CategorieCount = articleDbContext.Categories.Count(),
                TagCount = articleDbContext.Tags.Count(),
                NotificationCount = articleDbContext.Notifications.Count()
            };
            return View(viewModel);
        }

        private static byte[] GetData()
        {
            if (data == null)
            {
                data = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    data[i] = 1;
                }
            }
            return data;
        }

        [NoCache]
        [ForceWebSocket]
        public async Task<IActionResult> Pushing()
        {
            await pusher.Accept(HttpContext);
            for (int i = 0; i < 1800 && pusher.Connected; i++)
            {
                try
                {
                    await pusher.SendMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $"|{i + 1}");
                    await Task.Delay(100);
                }
                catch
                {
                    return NoContent();
                }
            }
            return Ok();
        }

        [NoCache]
        public IActionResult Download()
        {
            HttpContext.Response.Headers.Append("Content-Length", length.ToString());
            return new FileContentResult(GetData(), "application/octet-stream");
        }

        [NoCache]
        public IActionResult Ping() => Ok();
    }
} 