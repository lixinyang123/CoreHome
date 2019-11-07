using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace admin.Attributes
{
    public class ForceWebSocket : ActionFilterAttribute
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
