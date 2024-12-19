using CoreHome.Admin.Filter;
using CoreHome.Admin.ViewModels;
using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class BlogController(ArticleDbContext articleDbContext,
        OssService ossService,
        IMemoryCache memoryCache,
        IConfiguration configuration) : Controller
    {
        private readonly ArticleDbContext articleDbContext = articleDbContext;
        private readonly OssService ossService = ossService;
        private readonly IMemoryCache memoryCache = memoryCache;
        private readonly int pageSize = configuration.GetValue<int>("PageSize");

        //矫正页码
        private static int CorrectIndex(int index, int pageCount)
        {
            //页码<1时留在第一页
            index = index < 1 ? 1 : index;
            //页码>总页数时留在最后一页
            index = index > pageCount ? pageCount : index;
            //如果没有博客时留在第一页
            index = pageCount == 0 ? 1 : index;
            return index;
        }

        public async Task<IActionResult> Index(int index = 1)
        {
            ViewBag.PageTitle = "Blog";

            //博客总页数
            int pageCount = Convert.ToInt32(
                Math.Ceiling(
                    Convert.ToDouble(await articleDbContext.Articles.CountAsync()) / pageSize
                )
            );

            index = CorrectIndex(index, pageCount);

            ViewBag.CurrentIndex = index;
            ViewBag.PageCount = pageCount == 0 ? 1 : pageCount;

            List<Article> articles = await articleDbContext.Articles
                .AsNoTracking()
                .OrderByDescending(i => i.Id)
                .Skip((index - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(articles);
        }

        [HttpPost]
        public IActionResult Save(ArticleViewModel viewModel)
        {
            try
            {
                //暂存文章
                _ = memoryCache.Set("tempArticle", viewModel, DateTimeOffset.Now.AddDays(1));
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult Upload()
        {
            ViewBag.PageTitle = "Upload";
            ViewBag.Action = "Upload";

            //读取暂存文章
            if (!memoryCache.TryGetValue("tempArticle", out ArticleViewModel viewModel))
            {
                viewModel = new ArticleViewModel();
            }

            return View("Editor", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(ArticleViewModel articleViewModel)
        {
            ViewBag.PageTitle = "Upload";

            if (!ModelState.IsValid)
            {
                ViewBag.Action = "Upload";
                return View("Editor", articleViewModel);
            }

            List<ArticleTag> articleTags = [];
            new List<string>(articleViewModel.TagStr.Split("#").Distinct()).ForEach((i) =>
            {
                Tag tag = articleDbContext.Tags.SingleOrDefault(tag => tag.TagName == i);

                if (tag == null)
                {
                    articleTags.Add(new ArticleTag() { Tag = new Tag() { TagName = i } });
                }
                else
                {
                    articleTags.Add(new ArticleTag() { TagId = tag.Id });
                }
            });

            Category category = await articleDbContext.Categories
                .SingleOrDefaultAsync(i => i.CategoriesName == articleViewModel.CategoryName);

            category ??= new Category() { CategoriesName = articleViewModel.CategoryName };

            DateTime time = DateTime.Now;

            Year year = await articleDbContext.Years.SingleOrDefaultAsync(i => i.Value == time.Year);

            year ??= new Year() { Value = time.Year };

            Month month = await articleDbContext.Months.SingleOrDefaultAsync(i => i.Value == time.Month && i.Year.Value == time.Year);

            month ??= new Month() { Value = time.Month, Year = year };

            _ = articleDbContext.Articles.Add(new Article()
            {
                ArticleCode = Guid.NewGuid(),
                Title = articleViewModel.Title,
                Time = time,
                Month = month,
                Category = category,
                ArticleTags = articleTags,
                Overview = articleViewModel.Overview,
                Content = articleViewModel.Content
            });

            _ = articleDbContext.SaveChanges();

            //移除缓存
            memoryCache.Remove("tempArticle");

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Modify(Guid id)
        {
            Article article = await articleDbContext.Articles
                .AsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .SingleOrDefaultAsync(i => i.ArticleCode == id);

            ViewBag.PageTitle = article.Title;
            ViewBag.Action = "Modify";

            if (article == null)
            {
                return RedirectToAction("Index");
            }

            string tagStr = string.Empty;
            article.ArticleTags.ForEach(i =>
            {
                tagStr += i.Tag.TagName + "#";
            });

            ArticleViewModel articleViewModel = new()
            {
                ArticleCode = article.ArticleCode,
                Title = article.Title,
                CategoryName = article.Category.CategoriesName,
                TagStr = tagStr.TrimEnd('#'),
                Overview = article.Overview,
                Content = article.Content
            };

            return View("Editor", articleViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Modify(ArticleViewModel articleViewModel)
        {
            ViewBag.PageTitle = articleViewModel.Title;
            ViewBag.Action = "Modify";

            if (!ModelState.IsValid)
            {
                ViewBag.Warning = new
                {
                    Style = "alert alert-danger",
                    Content = "Please refine your article."
                };
                return View("Editor", articleViewModel);
            }

            ViewBag.Warning = new
            {
                Style = "alert alert-success",
                Content = "Modify article successful."
            };

            Article article = await articleDbContext.Articles.Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .SingleOrDefaultAsync(i => i.ArticleCode == articleViewModel.ArticleCode);

            ViewBag.PageTitle = article.Title;

            List<ArticleTag> articleTags = [];
            new List<string>(articleViewModel.TagStr.Split("#").Distinct()).ForEach((i) =>
            {
                Tag tag = articleDbContext.Tags.SingleOrDefault(tag => tag.TagName == i);

                if (tag == null)
                {
                    articleTags.Add(new ArticleTag() { Tag = new Tag() { TagName = i } });
                }
                else
                {
                    articleTags.Add(new ArticleTag() { TagId = tag.Id });
                }
            });

            Category category = await articleDbContext.Categories.SingleOrDefaultAsync(i =>
                i.CategoriesName == articleViewModel.CategoryName);

            category ??= new Category() { CategoriesName = articleViewModel.CategoryName };

            article.Title = articleViewModel.Title;
            article.Category = category;
            article.ArticleTags = articleTags;
            article.Overview = articleViewModel.Overview;
            article.Content = articleViewModel.Content;

            _ = articleDbContext.SaveChanges();

            RecyclingData();

            return View("Editor", articleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            Article article = await articleDbContext.Articles.Include(i => i.Comments)
                .Include(i => i.ArticleTags)
                .SingleOrDefaultAsync(i => i.ArticleCode == id);

            if (article != null)
            {
                _ = articleDbContext.Remove(article);
                article.ArticleTags.ForEach(i => articleDbContext.ArticleTags.Remove(i));
                article.Comments.ForEach(i => articleDbContext.Comments.Remove(i));
                _ = articleDbContext.SaveChanges();

                RecyclingData();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Comment(Guid id)
        {
            ViewBag.PageTitle = "Comment";

            Article article = await articleDbContext.Articles
                .AsNoTracking()
                .Include(i => i.Comments)
                .SingleOrDefaultAsync(i => i.ArticleCode == id);

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> DelComment(int id, Guid articleCode)
        {
            Comment comment = await articleDbContext.Comments.SingleOrDefaultAsync(i => i.Id == id);
            if (comment != null)
            {
                _ = articleDbContext.Comments.Remove(comment);
                _ = articleDbContext.SaveChanges();
            }
            return RedirectToAction("Comment", new { id = articleCode });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult UploadPic()
        {
            IFormFile file = HttpContext.Request.Form.Files["editormd-image-file"];
            using Stream stream = file.OpenReadStream();
            try
            {
                string uri = ossService.UploadBlogPic(stream);
                return Json(new { success = 1, message = "上传成功", url = uri });
            }
            catch (Exception ex)
            {
                return Json(new { success = 0, message = "上传失败" + ex.ToString(), url = string.Empty });
            }
        }

        private void RecyclingData(int times = 0)
        {
            //回收分类
            List<Category> noArticleCategories = [.. articleDbContext.Categories.Where(i => i.Articles.Count == 0)];
            noArticleCategories.ForEach(i => articleDbContext.Categories.Remove(i));

            //回收标签
            List<Tag> noArticleTag = [.. articleDbContext.Tags.Where(i => i.ArticleTags.Count == 0)];
            noArticleTag.ForEach(i => articleDbContext.Tags.Remove(i));

            //回收归档月份
            List<Month> months = [.. articleDbContext.Months.Where(i => i.Articles.Count == 0)];
            months.ForEach(i => articleDbContext.Months.Remove(i));

            //回收归档年份
            List<Year> years = [.. articleDbContext.Years.Where
            (
                i => i.Months.Count == 0 || (i.Months.Count == 1 && months.Contains(i.Months.First()))
            )];
            years.ForEach(i => articleDbContext.Years.Remove(i));

            try
            {
                _ = articleDbContext.SaveChanges();
            }
            catch
            {
                if (times > 2)
                    throw;
                RecyclingData(++times);
            }
        }
    }
}