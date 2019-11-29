using DataContext.CacheOperator;
using DataContext.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace admin.Attributes
{
    public sealed class DataChanged : ActionFilterAttribute
    {
        private readonly ICacheOperator<Article> articleCache;

        public DataChanged(ICacheOperator<Article> cacheOperator)
        {
            articleCache = cacheOperator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            articleCache.DelAllKeys();
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            articleCache.DelAllKeys();
            base.OnActionExecuted(context);
        }
    }
}
