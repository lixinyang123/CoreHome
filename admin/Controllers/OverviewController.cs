using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace admin.Controllers
{
    public class OverviewController : Controller
    {
        //验证方式
        //通过cookie.Get("admin")值获取session和cache中的accessToken进行对比

        public OverviewController(IMemoryCache cache)
        {
            //string admin = Request.Cookies["admin"];
            //ISession session = HttpContext.Session;
            //if(session.GetString(admin) != cache.Get<string>(admin))
            //{
            //    Response.Redirect("/Home");
            //}
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}