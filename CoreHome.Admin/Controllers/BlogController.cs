using CoreHome.Admin.Filter;
using CoreHome.Admin.ViewModels;
using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly OssService ossService;
        private readonly IMemoryCache memoryCache;
        private readonly int pageSize;

        public BlogController(ArticleDbContext articleDbContext,
            OssService ossService,
            IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            this.articleDbContext = articleDbContext;
            this.ossService = ossService;
            this.memoryCache = memoryCache;
            pageSize = configuration.GetValue<int>("PageSize");
        }

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
                memoryCache.Set("tempArticle", viewModel, DateTimeOffset.Now.AddDays(1));
                return Ok();
            }
            catch (Exception)
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
                viewModel = new ArticleViewModel();

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

            DateTime time = DateTime.Now;

            List<ArticleTag> articleTags = new();
            new List<string>(articleViewModel.TagStr.Split("#").Distinct()).ForEach((i) =>
            {
                Tag tag = articleDbContext.Tags.SingleOrDefault(tag => tag.TagName == i);

                if (tag == null)
                    articleTags.Add(new ArticleTag() { Tag = new Tag() { TagName = i } });
                else
                    articleTags.Add(new ArticleTag() { TagId = tag.Id });
            });

            Category category = await articleDbContext.Categories
                .SingleOrDefaultAsync(i => i.CategoriesName == articleViewModel.CategoryName);

            if (category == null)
                category = new Category() { CategoriesName = articleViewModel.CategoryName };

            Year year = await articleDbContext.Years.SingleOrDefaultAsync(i => i.Value == time.Year);

            if (year == null)
                year = new Year() { Value = time.Year };

            Month month = await articleDbContext.Months.SingleOrDefaultAsync(i => i.Value == time.Month && i.Year.Value == time.Year);

            if (month == null)
                month = new Month() { Value = time.Month, Year = year };

            articleDbContext.Articles.Add(new Article()
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

            articleDbContext.SaveChanges();

            //移除缓存
            memoryCache.Remove("tempArticle");

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Modify(Guid id)
        {
            Article article = await articleDbContext.Articles.Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .SingleOrDefaultAsync(i => i.ArticleCode == id);

            ViewBag.PageTitle = article.Title;

            if (article == null)
                return RedirectToAction("Index");

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

            ViewBag.Action = "Modify";
            return View("Editor", articleViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Modify(ArticleViewModel articleViewModel)
        {
            ViewBag.PageTitle = articleViewModel.Title;

            if (!ModelState.IsValid)
            {
                ViewBag.Action = "Modify";
                return View("Editor", articleViewModel);
            }

            Article article = await articleDbContext.Articles.Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .SingleOrDefaultAsync(i => i.ArticleCode == articleViewModel.ArticleCode);

            ViewBag.PageTitle = article.Title;

            List<ArticleTag> articleTags = new();
            new List<string>(articleViewModel.TagStr.Split("#").Distinct()).ForEach((i) =>
            {
                Tag tag = articleDbContext.Tags.SingleOrDefault(tag => tag.TagName == i);

                if (tag == null)
                    articleTags.Add(new ArticleTag() { Tag = new Tag() { TagName = i } });
                else
                    articleTags.Add(new ArticleTag() { TagId = tag.Id });
            });

            Category category = await articleDbContext.Categories.SingleOrDefaultAsync(i =>
                i.CategoriesName == articleViewModel.CategoryName);

            if (category == null)
                category = new Category() { CategoriesName = articleViewModel.CategoryName };

            article.Title = articleViewModel.Title;
            article.Category = category;
            article.ArticleTags = articleTags;
            article.Overview = articleViewModel.Overview;
            article.Content = articleViewModel.Content;

            articleDbContext.SaveChanges();

            RecyclingData();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            Article article = await articleDbContext.Articles.Include(i => i.Comments)
                .Include(i => i.ArticleTags)
                .SingleOrDefaultAsync(i => i.ArticleCode == id);

            if (article != null)
            {
                articleDbContext.Remove(article);
                article.ArticleTags.ForEach(i => articleDbContext.ArticleTags.Remove(i));
                article.Comments.ForEach(i => articleDbContext.Comments.Remove(i));
                articleDbContext.SaveChanges();

                RecyclingData();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Comment(Guid id)
        {
            ViewBag.PageTitle = "Comment";

            Article article = await articleDbContext.Articles
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
                articleDbContext.Comments.Remove(comment);
                articleDbContext.SaveChanges();
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

        private void RecyclingData()
        {
            //回收分类
            List<Category> noArticleCategories = articleDbContext.Categories.Where(i => i.Articles.Count == 0).ToList();
            noArticleCategories.ForEach(i => articleDbContext.Categories.Remove(i));

            //回收标签
            List<Tag> noArticleTag = articleDbContext.Tags.Where(i => i.ArticleTags.Count == 0).ToList();
            noArticleTag.ForEach(i => articleDbContext.Tags.Remove(i));

            //回收归档月份
            List<Month> months = articleDbContext.Months.Where(i => i.Articles.Count == 0).ToList();
            months.ForEach(i => articleDbContext.Months.Remove(i));

            articleDbContext.SaveChanges();

            //回收归档年份
            List<Year> years = articleDbContext.Years.Where(i => i.Months.Count == 0).ToList();
            years.ForEach(i => articleDbContext.Years.Remove(i));

            articleDbContext.SaveChanges();
        }

    }
}