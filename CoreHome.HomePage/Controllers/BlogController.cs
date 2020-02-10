using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Model;
using Infrastructure.common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coreHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly int pageSize = 5;
        private readonly ArticleDbContext articleDbContext;

        public BlogController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index(int index)
        {
            //获取页面起始页和结束页
            int count = articleDbContext.Articles.Count();
            index = PageManager.GetStartPageIndex(index, count, pageSize);
            ViewBag.LastPage = PageManager.GetLastPageIndex(count, pageSize);


            List<Article> articles = articleDbContext.Articles.Include(i => i.ArticleTags).Skip(index).Take(pageSize).ToList();

            GetTagList();

            if (articles.Count == 0)
            {
                ViewBag.Warning = "目前这里什么也没有";
            }

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];
            ViewBag.Pagination = "Index";
            return View(articles);
        }

        public IActionResult Search(string keyword, int index)
        {
            index = PageManager.GetStartPageIndex(index, articleDbContext.Articles.Count(), pageSize);

            List<Article> articles = articleDbContext.Articles.Include(i => i.ArticleTags).ThenInclude(i => i.Tag).Where(i => i.Title.Contains(keyword)).Skip(index).Take(pageSize).ToList();
            ViewBag.LastPage = PageManager.GetLastPageIndex(articles.Count, pageSize);

            GetTagList();

            ViewBag.Warning = keyword;

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];
            ViewBag.Pagination = "Search";
            return View("index", articles);
        }

        public IActionResult TagList(string tagName, int index)
        {
            Tag tag = articleDbContext.Tags.Include(i => i.ArticleTags).ThenInclude(i => i.Article).SingleOrDefault(i => i.TagName == tagName);

            //获取页面起始页和结束页
            index = PageManager.GetStartPageIndex(index, tag.ArticleTags.Count, pageSize);
            ViewBag.LastPage = PageManager.GetLastPageIndex(tag.ArticleTags.Count, pageSize);

            List<Article> articles = new List<Article>();
            tag.ArticleTags.ForEach(i =>
            {
                articles.Add(i.Article);
            });

            GetTagList();

            ViewBag.Warning = tagName;

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];
            ViewBag.Pagination = "TagList";
            return View("index", articles);
        }

        public IActionResult Detail(string articleID)
        {
            try
            {
                Article article = articleDbContext.Articles.Include(i => i.Comments).Include(i => i.ArticleTags).ThenInclude(i => i.Tag).Single(i => i.ArticleCode == articleID);

                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("lastRead", articleID, options);
                return View(article);
            }
            catch (Exception)
            {
                return Redirect("/Home/Message?msg=文章消失了&url=/Blog");
            }
        }

        public IActionResult Comment([FromForm]string id, [FromForm]string detail, [FromForm]string code)
        {
            ISession session = HttpContext.Session;
            string str = session.GetString("VerificationCode");

            if (!string.IsNullOrEmpty(code) && str == code.ToLower())
            {
                if (!string.IsNullOrEmpty(detail))
                {
                    Comment comment = new Comment()
                    {
                        Time = DateTime.Now,
                        Detail = detail,
                    };

                    articleDbContext.Articles.Include(i=>i.Comments).Single(i => i.ArticleCode == id).Comments.Add(comment);
                    articleDbContext.SaveChanges();
                    return Ok();
                }
                return ValidationProblem("内容不能为空");
            }
            return ValidationProblem("验证码错误");
        }

        public void GetTagList()
        {
            List<Tag> tags = articleDbContext.Tags.ToList();
            List<string> tagList = new List<string>();
            tags.ForEach(i =>
            {
                if (!tagList.Contains(i.TagName))
                {
                    if (!tagList.Contains(i.TagName))
                    {
                        tagList.Add(i.TagName);
                    }
                }
            });
            ViewBag.Tags = tagList;
        }

    }
}