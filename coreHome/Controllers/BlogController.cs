using DataContext.DbOperator;
using DataContext.Models;
using Infrastructure.common;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace coreHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly IDbOperator<Article> articleRepository;
        private readonly IDbOperator<Comment> commentRepository;
        private readonly int pageSize = 5;

        public BlogController(IWebHostEnvironment env,
            IDbOperator<Article> articleOperator,
            IDbOperator<Comment> commentOperator)
        {
            articleRepository = articleOperator;
            commentRepository = commentOperator;
            SearchEngineService.PushToBaidu(env.WebRootPath);
        }

        public IActionResult Index(int index)
        {
            index = PageManager.GetStartIndex(index, articleRepository.Count(), pageSize);
            List<Article> articles = articleRepository.Find(index, pageSize);

            //获取页面总数
            ViewBag.LastPage = PageManager.GetLastPage(articleRepository.Count(), pageSize);

            if (articles.Count == 0)
            {
                ViewBag.Warning = "目前这里什么也没有";
            }

            ViewBag.CurrentIndex = index;
            ViewBag.LastRead = Request.Cookies["lastRead"];
            return View(articles);
        }

        public IActionResult Detail(string articleID)
        {
            Article article = articleRepository.Find(articleID);
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
                return Redirect("/Home/Message?msg=请先同意隐私策略&url=/Blog/Detail?articleID=" + id);
            }

            if (code != null && code != string.Empty && str == code.ToLower())
            {
                if (detail != null && detail != string.Empty)
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

    }
}