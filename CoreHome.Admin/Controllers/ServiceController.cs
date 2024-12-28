using CoreHome.Admin.Filter;
using CoreHome.Admin.ViewModels;
using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ServiceController(ArticleDbContext articleDbContext) : Controller
    {
        private readonly ArticleDbContext articleDbContext = articleDbContext;

        public async Task<IActionResult> PreSearch(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(Array.Empty<object>());
            }

            List<Article> articles = await articleDbContext.Articles
                .AsNoTracking()
                .OrderByDescending(i => i.Id)
                .Where(i => i.Title.Contains(id, StringComparison.CurrentCultureIgnoreCase) || i.Overview.Contains(id, StringComparison.CurrentCultureIgnoreCase))
                .Take(5)
                .ToListAsync();

            List<PreSearchViewModel> viewModels = [.. articles.Select(i => new PreSearchViewModel(i.ArticleCode, i.Title, i.Overview))];
            return Json(viewModels);
        }
    }
}
