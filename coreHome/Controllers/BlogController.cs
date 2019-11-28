using DataContext.CacheOperator;
using DataContext.DbOperator;
using DataContext.Models;
using Infrastructure.common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coreHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly IDbOperator<Article> articleRepository;
        private readonly IDbOperator<Comment> commentRepository;
        private readonly IDbOperator<Tag> tagRepository;
        private readonly ICacheOperator<Article> articleCache;
        private readonly int pageSize = 5;

        public BlogController(IDbOperator<Article> articleOperator,
            IDbOperator<Comment> commentOperator,
            IDbOperator<Tag> tagOperator,
            ICacheOperator<Article> cacheOperator)
        {
            articleRepository = articleOperator;
            commentRepository = commentOperator;
            tagRepository = tagOperator;
            articleCache = cacheOperator;
        }

        public IActionResult Index(int index)
        {
            List<Article> articles = new List<Article>();

            string cacheKey = "index" + index;
            articles = articleCache.GetList(cacheKey);

            if (articles == null)
            {
                //获取页面起始页和结束页
                index = PageManager.GetStartPageIndex(index, articleRepository.Count(), pageSize);
                ViewBag.LastPage = PageManager.GetLastPageIndex(articleRepository.Count(), pageSize);

                articles = articleRepository.Find(i => i.Title != null, index, pageSize);
                articles.ForEach(i => i.Tags = tagRepository.Find(j => j.ArticleID == i.ArticleID, 0, tagRepository.Count()));

                articleCache.AddList(cacheKey, articles);
            }

            GetTagList();

            if (articles.Count == 0)
            {
                ViewBag.Warning = "目前这里什么也没有";
            }

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];
            return View(articles);
        }

        public IActionResult Search(string keyword, int index)
        {
            List<Article> articles = articleRepository.Find(i => i.Title.ToLower().Contains(keyword.ToLower()), 0, articleRepository.Count());

            index = PageManager.GetStartPageIndex(index, articles.Count, pageSize);
            ViewBag.LastPage = PageManager.GetLastPageIndex(articles.Count, pageSize);

            articles = articles.Skip(index * pageSize).Take(pageSize).ToList();
            articles.ForEach(i => i.Tags = tagRepository.Find(j => j.ArticleID == i.ArticleID, 0, tagRepository.Count()));

            GetTagList();

            ViewBag.Warning = keyword;

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];

            return View(articles);
        }

        public IActionResult TagList(string tagName, int index)
        {
            List<Tag> tags = tagRepository.Find(i => i.TagName.ToLower() == tagName.ToLower(), index, pageSize);

            //获取页面起始页和结束页
            index = PageManager.GetStartPageIndex(index, tags.Count, pageSize);
            ViewBag.LastPage = PageManager.GetLastPageIndex(tags.Count, pageSize);

            tags = tags.Skip(index * pageSize).Take(pageSize).ToList();

            List<Article> articles = new List<Article>();
            foreach (Tag tag in tags)
            {
                Article article = articleRepository.Find(tag.ArticleID);
                article.Tags = tagRepository.Find(i => i.ArticleID == article.ArticleID, 0, tagRepository.Count());
                articles.Add(article);
            }

            GetTagList();

            ViewBag.Warning = tagName;

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];
            return View(articles);
        }

        public IActionResult Detail(string articleID)
        {
            Article article = articleRepository.Find(articleID);
            article.Comments = commentRepository.Find(i => i.ArticleID == articleID, 0, commentRepository.Count());
            article.Tags = tagRepository.Find(i => i.ArticleID == articleID, 0, tagRepository.Count());
            if (article != null)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("lastRead", articleID, options);
                return View(article);
            }
            else
            {
                return Content("没有此文章");
            }
        }

        public IActionResult Comment([FromForm]string id, [FromForm]string detail, [FromForm]string code)
        {
            ISession session = HttpContext.Session;
            string str = session.GetString("Verification");
            if (str == null)
            {
                return Redirect("/Home/Message?msg=请先同意底部隐私策略&url=/Blog/Detail?articleID=" + id);
            }

            if (!string.IsNullOrEmpty(code) && str == code.ToLower())
            {
                if (!string.IsNullOrEmpty(detail))
                {
                    Comment comment = new Comment()
                    {
                        CommentID = Guid.NewGuid().ToString(),
                        Time = DateTime.Now.ToString(),
                        Detail = detail,
                        ArticleID = id
                    };

                    commentRepository.Add(comment);
                    return Redirect("/Home/Message?msg=评论成功&url=/Blog/Detail?articleID=" + id);
                }
                return Redirect("/Home/Message?msg=评论不能为空&url=/Blog/Detail?articleID=" + id);
            }
            return Redirect("/Home/Message?msg=验证码错误&url=/Blog/Detail?articleID=" + id);
        }

        public void GetTagList()
        {
            List<Tag> tags = tagRepository.Find(i => i.ArticleID != null, 0, tagRepository.Count());
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