using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using CoreHome.Data.Model;
using CoreHome.HomePage.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Http;

namespace CoreHome.HomePage.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly int pageSize;

        public BlogController(ArticleDbContext articleDbContext,IConfiguration configuration)
        {
            this.articleDbContext = articleDbContext;
            pageSize = configuration.GetValue<int>("PageSize");
        }

        //矫正页码
        private int CorrectIndex(int index, int pageCount)
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
        public IActionResult Index(int index = 1)
        {
            //博客总页数
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(articleDbContext.Articles.Count()) / pageSize));
            index = CorrectIndex(index, pageCount);

            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i => i.Id)
                .Include(i => i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Skip((index - 1) * pageSize)
                .Take(pageSize).ToList();

            ViewBag.Pagination = new PaginationViewModel()
            {
                CurrentIndex = index,
                PageCount = pageCount
            };

            ViewBag.Warning = "All Posts";
            return View(articles);
        }

        /// <param name="id">博客类别</param>
        public IActionResult Categories(string id)
        {
            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i => i.Id)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Category.CategoriesName == id)
                .ToList();

            ViewBag.Warning = id;
            return View("Index", articles);
        }

        /// <param name="id">博客标签</param>
        public IActionResult Tags(string id)
        {
            List<ArticleTag> articleTags = articleDbContext.ArticleTags
                .OrderByDescending(i => i.Article.Id)
                .Include(i => i.Article)
                .ThenInclude(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Tag.TagName == id).ToList();

            List<Article> articles = new List<Article>();
            articleTags.ForEach(i => articles.Add(i.Article));

            ViewBag.Warning = id;
            return View("Index", articles);
        }

        public IActionResult Archive(int id,int para)
        {
            List<Article> articles = articleDbContext.Months
                .Include(i=>i.Year)
                .Include(i => i.Articles)
                .ThenInclude(i=>i.ArticleTags)
                .ThenInclude(i=>i.Tag)
                .SingleOrDefault(i => i.Year.Value == id && i.Value == para)
                .Articles.ToList();

            ViewBag.Warning = id + "/" + para;
            return View("Index", articles);
        }

        [HttpPost]
        public IActionResult Search(string keyword)
        {
            if (keyword == null)
            {
                return RedirectToAction("Index");
            }

            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i => i.Id)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Title.ToLower().Contains(keyword.ToLower()))
                .ToList();

            ViewBag.Warning = keyword;
            return View("Index", articles);
        }

        public IActionResult Detail(Guid id)
        {
            Article article = articleDbContext.Articles
                .Include(i => i.Category)
                .Include(i => i.Comments)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .SingleOrDefault(i => i.ArticleCode == id);

            return View(new DetailViewModel()
            {
                Article = article,
                CommentViewModel = new CommentViewModel()
            });
        }

        [HttpPost]
        public IActionResult Detail(CommentViewModel viewModel)
        {
            Article article = articleDbContext.Articles
                   .Include(i => i.Category)
                   .Include(i => i.Comments)
                   .Include(i => i.ArticleTags)
                   .ThenInclude(i => i.Tag)
                   .SingleOrDefault(i => i.ArticleCode == viewModel.ArticleCode);

            DetailViewModel detailViewModel = new DetailViewModel()
            {
                Article = article,
                CommentViewModel = viewModel
            };

            if (ModelState.IsValid)
            {
                string str = HttpContext.Session.GetString("VerificationCode");
                if (str == viewModel.VerificationCode.ToLower())
                {
                    article.Comments.Add(new Comment()
                    {
                        Time = DateTime.Now,
                        Detail = viewModel.Detail
                    });
                    articleDbContext.SaveChanges();

                    detailViewModel.CommentViewModel = new CommentViewModel();
                    ViewBag.Warning = "评论成功";
                    return View(detailViewModel);
                }
                else
                {
                    ViewBag.Warning = "验证码错误";
                    return View(detailViewModel);
                }
            }

            ViewBag.Warning = "请完善评论";
            return View(detailViewModel);
        }
    }
}