using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace admin.Controllers
{
    public class OverviewController : Controller
    {
        public OverviewController(IMemoryCache _cache)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}