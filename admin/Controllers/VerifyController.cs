using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace admin.Controllers
{
    public class VerifyController : Controller
    {
        //验证方式
        //通过cookie.Get("admin")值获取session和cache中的accessToken进行对比

        public readonly IMemoryCache cache;

        public VerifyController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string admin = Request.Cookies["admin"];
                ISession session = HttpContext.Session;
                string sessionStr = session.GetString(admin);
                string cacheStr = cache.Get<string>(admin);
                if (sessionStr != cacheStr || sessionStr == null || cacheStr == null)
                {
                    //验证访问令牌失败直接撤销管理员权限
                    Response.Cookies.Delete("admin");
                    context.HttpContext.Response.Redirect("/Admin/Home");
                }
            }
            catch (Exception)
            {
                context.HttpContext.Response.Redirect("/Admin/Home");
            }

            base.OnActionExecuting(context);
        }

    }
}