using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using CoreHome.Data.Model;

namespace CoreHome.HomePage.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public ArchiveController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Archive";

            List<Year> years = articleDbContext.Years
                .Include(i => i.Months)
                .ThenInclude(i => i.Articles)
                .ToList();

            return View(years);
        }
    }
}