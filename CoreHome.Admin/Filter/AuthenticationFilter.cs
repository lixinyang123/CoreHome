using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;

namespace CoreHome.Admin.Filter
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public readonly IMemoryCache cache;
        private readonly IWebHostEnvironment environment;

        public AuthorizationFilter(IMemoryCache cache, IWebHostEnvironment environment)
        {
            this.cache = cache;
            this.environment = environment;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!environment.IsDevelopment())
            {
                IdentityAuthorization(context);
            }
        }

        /// <summary>
        /// 身份验证
        /// </summary>
        public void IdentityAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string tokenStr = context.HttpContext.Request.Cookies["accessToken"];

                string cacheKey = context.HttpContext.Request.Cookies["user"];
                string cacheStr = cache.Get<string>(cacheKey);

                if (tokenStr != cacheStr || tokenStr == null || cacheStr == null)
                {
                    throw new Exception("AuthenticationException");
                }
            }
            catch (Exception)
            {
                //验证访问令牌失败直接撤销管理员权限
                context.HttpContext.Response.Cookies.Delete("accessToken");
                context.Result = new ObjectResult("Permission denied");
            }
        }

    }
}
