using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreHome.Admin.Attributes
{
    public sealed class NoCache : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Response.Headers.ContainsKey("Cache-Control"))
            {
                context.HttpContext.Response.Headers.Append("Cache-Control", "no-cache");
            }
            if (!context.HttpContext.Response.Headers.ContainsKey("Expires"))
            {
                context.HttpContext.Response.Headers.Append("Expires", "-1");
            }
            base.OnActionExecuting(context);
        }
    }
}
