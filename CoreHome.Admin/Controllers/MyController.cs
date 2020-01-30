using admin.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;

namespace admin.Controllers
{
    public class MyController : Controller
    {
        //验证方式
        //通过session中存的accessToken和cache中放行的accessToken进行对比

        public readonly IMemoryCache cache;
        private readonly IWebHostEnvironment environment;

        public MyController(IMemoryCache _cache, IWebHostEnvironment env)
        {
            cache = _cache;
            environment = env;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Type type = context.Controller.GetType();
            System.Collections.Generic.IEnumerable<System.Reflection.CustomAttributeData> attributes = type.CustomAttributes;
            foreach (System.Reflection.CustomAttributeData attribute in attributes)
            {
                //身份验证
                //开发者模式不进行身份验证
                if (attribute.AttributeType.Equals(typeof(Authorization)) && !environment.IsDevelopment())
                {
                    IdentityAuthorization(context);
                }
            }

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 身份验证
        /// </summary>
        public void IdentityAuthorization(ActionExecutingContext context)
        {
            try
            {
                string tokenStr = Request.Cookies["accessToken"];

                string cacheKey = Request.Cookies["user"];
                string cacheStr = cache.Get<string>(cacheKey);

                if (tokenStr != cacheStr || tokenStr == null || cacheStr == null)
                {
                    throw new Exception("AuthenticationException");
                }
            }
            catch (Exception)
            {
                //验证访问令牌失败直接撤销管理员权限
                Response.Cookies.Delete("accessToken");
                context.Result = Redirect("/Admin");
            }
        }

    }
}