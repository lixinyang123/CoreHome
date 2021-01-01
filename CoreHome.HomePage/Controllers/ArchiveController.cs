using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                .OrderByDescending(i => i.Value)
                .Include(i => i.Months)
                .ThenInclude(i => i.Articles)
                .ToList();

            return View(years);
        }
    }
}