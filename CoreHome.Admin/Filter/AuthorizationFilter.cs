using CoreHome.Admin.Services;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreHome.Admin.Filter
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly IWebHostEnvironment environment;
        private readonly SecurityService securityService;
        private readonly ProfileService profileService;

        public AuthorizationFilter(IWebHostEnvironment environment, SecurityService securityService, ProfileService profileService)
        {
            this.environment = environment;
            this.securityService = securityService;
            this.profileService = profileService;
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
            // 未设定管理员密码，不进行认证
            if (string.IsNullOrEmpty(profileService.Config.AdminPassword))
            {
                return;
            }

            try
            {
                string tokenStr = context.HttpContext.Request.Cookies["accessToken"];
                string cacheKey = context.HttpContext.Request.Cookies["user"];
                bool verifyDefeat = securityService.AESDecrypt(tokenStr) != cacheKey;

                if (verifyDefeat || string.IsNullOrEmpty(tokenStr) || string.IsNullOrEmpty(cacheKey))
                {
                    throw new Exception("AuthenticationException");
                }
            }
            catch
            {
                //验证访问令牌失败直接撤销管理员权限
                context.HttpContext.Response.Cookies.Delete("accessToken");

                context.Result = new RedirectToActionResult("Index", "Home", new
                {
                    Redirect = context.HttpContext.Request.PathBase + context.HttpContext.Request.Path
                });
            }
        }
    }
}
