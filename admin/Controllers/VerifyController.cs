using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace admin.Controllers
{
    public class VerifyController : Controller
    {
        //验证方式
        //通过session中存的accessToken和cache中放行的accessToken进行对比

        public readonly IMemoryCache cache;
        private readonly IWebHostEnvironment environment;

        public VerifyController(IMemoryCache _cache, IWebHostEnvironment env)
        {
            cache = _cache;
            environment = env;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //开发者模式不进行身份验证
            if (!environment.IsDevelopment())
            {
                try
                {
                    ISession session = HttpContext.Session;
                    string sessionStr = session.GetString("accessToken");

                    string cacheKey = Request.Cookies["user"];
                    string cacheStr = cache.Get<string>(cacheKey);

                    if (sessionStr != cacheStr || sessionStr == null || cacheStr == null)
                    {
                        //验证访问令牌失败直接撤销管理员权限
                        context.HttpContext.Response.Redirect("/Admin");
                    }
                }
                catch (Exception)
                {
                    context.HttpContext.Response.Redirect("/Admin");
                }
            }

            base.OnActionExecuting(context);
        }

    }
}