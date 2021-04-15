using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using CoreHome.HomePage.ViewModels;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHome.HomePage.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly NotifyService notifyService;
        private readonly int pageSize;

        public BlogController(ArticleDbContext articleDbContext, IConfiguration configuration, NotifyService notifyService)
        {
            this.articleDbContext = articleDbContext;
            this.notifyService = notifyService;
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

        /// <param name="index">页面索引</param>
        public async Task<IActionResult> Index(int index = 1)
        {
            ViewBag.PageTitle = "Blogs";

            //博客总页数
            int pageCount = await Task.Run(() =>
            {
                int count = articleDbContext.Articles.Count();
                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });
            index = CorrectIndex(index, pageCount);

            List<Article> articles = await articleDbContext.Articles
                .OrderByDescending(i => i.Id)
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.Pagination = new PaginationViewModel()
            {
                CurrentIndex = index,
                PageCount = pageCount,
                ActionName = "Index"
            };

            ViewBag.Warning = "All Posts";
            return View(articles);
        }

        /// <param name="id">博客类别</param>
        public async Task<IActionResult> Categories(string id, int index = 1)
        {
            ViewBag.PageTitle = id;

            int pageCount = await Task.Run(() =>
            {
                int count = articleDbContext.Articles.Where(i => i.Category.CategoriesName == id).Count();
                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });
            index = CorrectIndex(index, pageCount);

            List<Article> articles = await articleDbContext.Articles
                .OrderByDescending(i => i.Id)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Category.CategoriesName == id)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.Pagination = new PaginationViewModel()
            {
                CurrentIndex = index,
                PageCount = pageCount,
                ActionName = "Categories"
            };

            ViewBag.Warning = id;
            return View("Index", articles);
        }

        /// <param name="id">博客标签</param>
        public async Task<IActionResult> Tags(string id, int index = 1)
        {
            ViewBag.PageTitle = id;

            int pageCount = await Task.Run(() =>
            {
                int count = articleDbContext.ArticleTags.Include(i => i.Tag).Where(i => i.Tag.TagName == id).Count();
                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });
            index = CorrectIndex(index, pageCount);

            List<ArticleTag> articleTags = await articleDbContext.ArticleTags
                .OrderByDescending(i => i.Article.Id)
                .Include(i => i.Article)
                .ThenInclude(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Tag.TagName == id)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            List<Article> articles = new();
            articleTags.ForEach(i => articles.Add(i.Article));

            ViewBag.Pagination = new PaginationViewModel()
            {
                CurrentIndex = index,
                PageCount = pageCount,
                ActionName = "Tags"
            };

            ViewBag.Warning = id;
            return View("Index", articles);
        }

        public async Task<IActionResult> Archive(int id, int para, int index = 1)
        {
            if (id == 0 || para == 0)
                return NotFound();

            string date = $"{id}/{para}";
            ViewBag.PageTitle = date;

            int pageCount = await Task.Run(() =>
            {
                int count = articleDbContext.Articles.Where(i => i.Month.Value == para && i.Month.Year.Value == id).Count();
                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });
            index = CorrectIndex(index, pageCount);

            List<Article> articles = await articleDbContext.Articles
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Month.Value == para && i.Month.Year.Value == id)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.Pagination = new PaginationViewModel()
            {
                CurrentIndex = index,
                PageCount = pageCount,
                ActionName = "Archive"
            };

            ViewBag.Warning = date;
            return View("Index", articles);
        }

        public async Task<IActionResult> Search(string id, int index = 1)
        {
            ViewBag.PageTitle = id;

            if (id == null)
                return RedirectToAction("Index");

            int pageCount = await Task.Run(() =>
            {
                int count = articleDbContext.Articles.Where(i => i.Title.ToLower().Contains(id.ToLower())).Count();
                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });
            index = CorrectIndex(index, pageCount);

            List<Article> articles = await articleDbContext.Articles
                .OrderByDescending(i => i.Id)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Title.ToLower().Contains(id.ToLower()))
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.Pagination = new PaginationViewModel()
            {
                CurrentIndex = index,
                PageCount = pageCount,
                ActionName = "Search"
            };

            ViewBag.Warning = id;
            return View("Index", articles);
        }

        /// <summary>
        /// 查看博客详情
        /// </summary>
        /// <param name="id">博客ArticleCode</param>
        /// <returns>博客详情页面</returns>
        public async Task<IActionResult> Detail(Guid id)
        {
            Article article = await articleDbContext.Articles
                .Include(i => i.Category)
                .Include(i => i.Comments)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .SingleOrDefaultAsync(i => i.ArticleCode == id);

            if (article == null)
                return NotFound();

            ViewBag.PageTitle = article.Title;

            return View(new DetailViewModel()
            {
                Article = article,
                CommentViewModel = new CommentViewModel()
            });
        }

        /// <summary>
        /// 评论功能
        /// </summary>
        /// <param name="viewModel">评论内容</param>
        /// <returns>评论状态</returns>
        [HttpPost]
        public async Task<IActionResult> Detail(CommentViewModel viewModel)
        {
            Article article = await articleDbContext.Articles
                   .Include(i => i.Category)
                   .Include(i => i.Comments)
                   .Include(i => i.ArticleTags)
                   .ThenInclude(i => i.Tag)
                   .SingleOrDefaultAsync(i => i.ArticleCode == viewModel.ArticleCode);

            if (article == null)
                return RedirectToAction("Index");

            ViewBag.PageTitle = article.Title;

            DetailViewModel detailViewModel = new()
            {
                Article = article,
                CommentViewModel = viewModel
            };

            if (!ModelState.IsValid)
            {
                ViewBag.Warning = "Please refine your Comment";
                return View(detailViewModel);
            }

            string str = HttpContext.Session.GetString("VerificationCode");
            if (str == viewModel.VerificationCode.ToLower())
            {
                article.Comments.Add(new Comment()
                {
                    Time = DateTime.Now,
                    Detail = viewModel.Detail
                });
                articleDbContext.SaveChanges();

                //评论通知
                notifyService.PushNotify($"New Comment for [{article.Title}]", viewModel.Detail);

                detailViewModel.CommentViewModel = new CommentViewModel();
                ViewBag.Warning = "Thank you for your Comment";
                return View(detailViewModel);
            }
            else
            {
                ViewBag.Warning = "The verification code is wrong";
                return View(detailViewModel);
            }

        }
    }
}