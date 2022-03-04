using CoreHome.Admin.Filter;
using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class NotificationController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public NotificationController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Comment";
            return View(
                articleDbContext.Notifications.OrderByDescending(i => i.Id).ToList()
            );
        }

        public async Task<IActionResult> Delete(int id)
        {
            articleDbContext.Notifications.Remove(articleDbContext.Notifications.Single(i => i.Id == id));
            await articleDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAll()
        {
            articleDbContext.Notifications.RemoveRange(articleDbContext.Notifications);
            await articleDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
