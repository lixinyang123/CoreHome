using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.HomePage.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public ArchiveController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PageTitle = "Archive";

            List<Year> years = await articleDbContext.Years
                .OrderByDescending(i => i.Value)
                .Include(i => i.Months)
                .ThenInclude(i => i.Articles)
                .ToListAsync();

            return View(years);
        }
    }
}