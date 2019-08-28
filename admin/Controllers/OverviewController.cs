using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace admin.Controllers
{
    public class OverviewController : VerifyController
    {
        public OverviewController(IMemoryCache _cache) : base(_cache) { }

        public IActionResult Index()
        {
            return View();
        }
    }
}