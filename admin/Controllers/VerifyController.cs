﻿using System;
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

        private readonly IMemoryCache cache;

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
                if (sessionStr != cacheStr)
                {
                    context.HttpContext.Response.Redirect("/Home");
                }
            }
            catch (Exception)
            {
                context.HttpContext.Response.Redirect("/Home");
            }

            base.OnActionExecuting(context);
        }

    }
}