using CoreHome.Admin.Filter;
using CoreHome.Admin.ViewModels;
using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly OssService ossService;

        public BlogController(ArticleDbContext articleDbContext, OssService ossService)
        {
            this.articleDbContext = articleDbContext;
            this.ossService = ossService;
        }

        public IActionResult Index()
        {
            ViewBag.PageTitle = "Blog";

            List<Article> articles = articleDbContext.Articles.OrderByDescending(i => i.Id).ToList();
            return View(articles);
        }

        public IActionResult Upload()
        {
            ViewBag.PageTitle = "Upload";

            ViewBag.Action = "Upload";
            return View("Editor", new ArticleViewModel());
        }

        [HttpPost]
        public IActionResult Upload(ArticleViewModel articleViewModel)
        {
            ViewBag.PageTitle = "Upload";

            if (!ModelState.IsValid)
            {
                ViewBag.Action = "Upload";
                return View("Editor", articleViewModel);
            }

            DateTime time = DateTime.Now;

            List<ArticleTag> articleTags = new List<ArticleTag>();
            new List<string>(articleViewModel.TagStr.Split("#").Distinct()).ForEach(i =>
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

            Category category = articleDbContext.Categories
                .SingleOrDefault(i => i.CategoriesName == articleViewModel.CategoryName);

            if (category == null)
            {
                category = new Category() { CategoriesName = articleViewModel.CategoryName };
            }

            Month month = articleDbContext.Months.SingleOrDefault(i => i.Value == time.Month);

            if (month == null)
            {
                Year year = articleDbContext.Years.SingleOrDefault(i => i.Value == time.Year);
                if (year == null)
                {
                    year = new Year() { Value = time.Year };
                }

                month = new Month() { Value = time.Month, Year = year };
            }

            articleDbContext.Articles.Add(new Article()
            {
                ArticleCode = Guid.NewGuid(),
                Title = articleViewModel.Title,
                Time = time,
                Month = month,
                Category = category,
                ArticleTags = articleTags,
                Overview = articleViewModel.Overview,
                Content = Regex.Escape(articleViewModel.Content)
            });

            articleDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Modify(Guid id)
        {
            Article article = articleDbContext.Articles.Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .SingleOrDefault(i => i.ArticleCode == id);

            ViewBag.PageTitle = article.Title;

            if (article == null)
            {
                return RedirectToAction("Index");
            }

            string tagStr = string.Empty;
            article.ArticleTags.ForEach(i =>
            {
                tagStr += i.Tag.TagName + "#";
            });

            ArticleViewModel articleViewModel = new ArticleViewModel()
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
        public IActionResult Modify(ArticleViewModel articleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Editor", articleViewModel);
            }

            Article article = articleDbContext.Articles.Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .SingleOrDefault(i => i.ArticleCode == articleViewModel.ArticleCode);

            ViewBag.PageTitle = article.Title;

            List<ArticleTag> articleTags = new List<ArticleTag>();
            new List<string>(articleViewModel.TagStr.Split("#").Distinct()).ForEach(i =>
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

            Category category = articleDbContext.Categories.SingleOrDefault(i => i.CategoriesName == articleViewModel.CategoryName);
            if (category == null)
            {
                category = new Category() { CategoriesName = articleViewModel.CategoryName };
            }

            article.Title = articleViewModel.Title;
            article.Category = category;
            article.ArticleTags = articleTags;
            article.Overview = articleViewModel.Overview;
            article.Content = Regex.Unescape(articleViewModel.Content);

            articleDbContext.SaveChanges();

            RecyclingData();

            ViewBag.Action = "Modify";
            return View("Editor", articleViewModel);
        }

        public IActionResult Delete(Guid id)
        {
            Article article = articleDbContext.Articles.Include(i => i.Comments)
                .Include(i => i.ArticleTags)
                .SingleOrDefault(i => i.ArticleCode == id);

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

        public IActionResult Comment(Guid id)
        {
            ViewBag.PageTitle = "Comment";

            Article article = articleDbContext.Articles
                .Include(i => i.Comments)
                .SingleOrDefault(i => i.ArticleCode == id);
            return View(article);
        }

        public IActionResult DelComment(int id, Guid articleCode)
        {
            Comment comment = articleDbContext.Comments.SingleOrDefault(i => i.Id == id);
            if (comment != null)
            {
                articleDbContext.Comments.Remove(comment);
                articleDbContext.SaveChanges();
            }
            return RedirectToAction("Comment", new { id = articleCode });
        }

        [HttpPost]
        public IActionResult UploadPic()
        {
            IFormFile file = HttpContext.Request.Form.Files["editormd-image-file"];
            using Stream stream = file.OpenReadStream();
            try
            {
                string uri = ossService.UploadBlogPic(file.FileName, stream);
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
            articleDbContext.SaveChanges();

            //回收标签
            List<Tag> noArticleTag = articleDbContext.Tags.Where(i => i.ArticleTags.Count == 0).ToList();
            noArticleTag.ForEach(i => articleDbContext.Tags.Remove(i));
            articleDbContext.SaveChanges();

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