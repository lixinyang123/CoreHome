using CoreHome.Admin.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;

namespace CoreHome.Admin.Filter
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly IWebHostEnvironment environment;
        private readonly SecurityService securityService;

        public AuthorizationFilter(IWebHostEnvironment environment, SecurityService securityService)
        {
            this.environment = environment;
            this.securityService = securityService;
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
                bool verifyResult = securityService.Decrypt(tokenStr) != cacheKey;

                if (verifyResult || tokenStr == null || cacheKey == null)
                {
                    throw new Exception("AuthenticationException");
                }
            }
            catch (Exception)
            {
                //验证访问令牌失败直接撤销管理员权限
                context.HttpContext.Response.Cookies.Delete("accessToken");
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }

    }
}
