using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreHome.Admin.Attributes
{
    public sealed class ForceWebSocket : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.HttpContext.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            string arg = "Wrong protocal!";
            context.Result = new JsonResult(arg);
        }
    }
}
