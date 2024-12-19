﻿using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using CoreHome.HomePage.ViewModels;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreHome.HomePage.Controllers
{
    public class BlogController(ArticleDbContext articleDbContext, IConfiguration configuration, NotifyService notifyService) : Controller
    {
        private readonly ArticleDbContext articleDbContext = articleDbContext;
        private readonly NotifyService notifyService = notifyService;
        private readonly int pageSize = configuration.GetValue<int>("PageSize");

        /// <summary>
        /// 矫正页码
        /// </summary>
        /// <param name="index">当前页码</param>
        /// <param name="pageCount">可以查询到的总页数</param>
        /// <returns>矫正后的页码</returns>
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

        /// <summary>
        /// 博客列表
        /// </summary>
        /// <param name="index">页码</param>
        /// <returns>博客列表页面</returns>
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
                .AsNoTracking()
                .OrderByDescending(i => i.Id)
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.Pagination = new PaginationViewModel(index, pageCount, "Index");

            ViewBag.Warning = "All Posts";
            return View(articles);
        }

        /// <summary>
        /// 博客类别
        /// </summary>
        /// <param name="id">类别名称</param>
        /// <param name="index">页码</param>
        /// <returns>博客类别页面</returns>
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
                .AsNoTracking()
                .OrderByDescending(i => i.Id)
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Category.CategoriesName == id)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.Pagination = new PaginationViewModel(index, pageCount, "Categories");

            ViewBag.Warning = id;
            return View("Index", articles);
        }

        /// <summary>
        /// 博客标签
        /// </summary>
        /// <param name="id">博客标签</param>
        /// <param name="index">页码</param>
        /// <returns>博客标签页面</returns>
        public async Task<IActionResult> Tags(string id, int index = 1)
        {
            ViewBag.PageTitle = id;

            int pageCount = await Task.Run(() =>
            {
                int count = articleDbContext.ArticleTags
                    .AsNoTracking()
                    .Include(i => i.Tag)
                    .Count(i => i.Tag.TagName == id);

                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });
            index = CorrectIndex(index, pageCount);

            List<ArticleTag> articleTags = await articleDbContext.ArticleTags
                .OrderByDescending(i => i.Article.Id)
                .Include(i => i.Article)
                .Include(i => i.Article.Category)
                .Include(i => i.Article.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Tag.TagName == id)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            List<Article> articles = articleTags.Select(i => i.Article).ToList();
            ViewBag.Pagination = new PaginationViewModel(index, pageCount, "Tags");

            ViewBag.Warning = id;
            return View("Index", articles);
        }

        /// <summary>
        /// 归档页面
        /// </summary>
        /// <param name="id">年份</param>
        /// <param name="para">月份</param>
        /// <param name="index">页码</param>
        /// <returns>博客归档页面</returns>
        public async Task<IActionResult> Archive(int id, int para, int index = 1)
        {
            if (id == 0 || para == 0)
            {
                return NotFound();
            }

            string date = $"{id}/{para}";
            ViewBag.PageTitle = date;

            int pageCount = await Task.Run(() =>
            {
                int count = articleDbContext.Articles
                    .AsNoTracking()
                    .Count(i => i.Month.Value == para && i.Month.Year.Value == id);

                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });
            index = CorrectIndex(index, pageCount);

            List<Article> articles = await articleDbContext.Articles
                .AsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Month.Value == para && i.Month.Year.Value == id)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.Pagination = new PaginationViewModel(index, pageCount, "Archive");

            ViewBag.Warning = date;
            return View("Index", articles);
        }

        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <param name="id">搜索关键词</param>
        /// <param name="index">页码</param>
        /// <returns>搜索结果页面</returns>
        public async Task<IActionResult> Search(string id, int index = 1)
        {
            ViewBag.PageTitle = id;

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            int pageCount = await Task.Run(() =>
            {
                int count = articleDbContext.Articles
                    .AsNoTracking()
                    .Count(i => i.Title.Contains(id, StringComparison.OrdinalIgnoreCase));

                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });

            index = CorrectIndex(index, pageCount);

            List<Article> articles = await articleDbContext.Articles
                .AsNoTracking()
                .OrderByDescending(i => i.Id)
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i =>
                    i.Title.Contains(id, StringComparison.OrdinalIgnoreCase) ||
                    i.Overview.Contains(id, StringComparison.OrdinalIgnoreCase)
                )
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.Pagination = new PaginationViewModel(index, pageCount, "Search");

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
                .AsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.Comments)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .SingleOrDefaultAsync(i => i.ArticleCode == id);

            if (article == null)
            {
                return NotFound();
            }

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
            {
                return RedirectToAction("Index");
            }

            ViewBag.PageTitle = article.Title;

            DetailViewModel detailViewModel = new()
            {
                Article = article,
                CommentViewModel = viewModel
            };

            if (!ModelState.IsValid)
            {
                ViewBag.Warning = new
                {
                    Style = "alert alert-danger",
                    Content = "Please refine your Comment."
                };
                return View(detailViewModel);
            }

            string str = HttpContext.Session.GetString("VerificationCode");
            if (str != viewModel.VerificationCode.ToLower())
            {
                ViewBag.Warning = new
                {
                    Style = "alert alert-danger",
                    Content = "The verification code is wrong."
                };
                return View(detailViewModel);
            }

            article.Comments.Add(new Comment()
            {
                Time = DateTime.Now,
                Email = viewModel.Email,
                Detail = viewModel.Detail
            });

            string title = "[ New Comment ]";
            string content = $"# Blog \n{article.Title} \n# Detail \n{viewModel.Detail}";

            _ = await articleDbContext.Notifications.AddAsync(new Notification(title, content));
            _ = await articleDbContext.SaveChangesAsync();

            notifyService.PushNotify(
                title,
                content,
                $"{Request.Headers.Origin}/Admin/Blog/Comment/{article.ArticleCode}"
            );

            detailViewModel.CommentViewModel = new CommentViewModel();
            ViewBag.Warning = new
            {
                Style = "alert alert-success",
                Content = "Thank you for your Comment."
            };

            return View(detailViewModel);
        }
    }
}