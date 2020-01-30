using CoreHome.Admin.Filter;
using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public BlogController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index()
        {
            List<Article> articles = articleDbContext.Articles.Include(i => i.ArticleTags).ThenInclude(i => i.Tag).ToList();
            return View(articles);
        }

        public IActionResult UploadArticle()
        {
            if (Request.Method == "POST")
            {
                List<ArticleTag> articleTags = new List<ArticleTag>();
                List<string> tags = new List<string>(Request.Form["tag"].ToString().TrimStart('#').Split("#"));
                tags.ForEach(i =>
                {
                    Tag tag = articleDbContext.Tags.SingleOrDefault(x => x.TagName == i);
                    if (tag == null)
                    {
                        articleTags.Add(new ArticleTag() { Tag = new Tag() { TagName = i } });
                    }
                    else
                    {
                        articleTags.Add(new ArticleTag() { TagId = tag.Id });
                    }
                });

                //添加博客记录
                Article article = new Article()
                {
                    ArticleCode = Guid.NewGuid().ToString(),
                    Title = Request.Form["title"],
                    Time = DateTime.Now,
                    CoverUrl = Request.Form["cover"],
                    Overview = Request.Form["overview"],
                    Content = Request.Form["content"],
                    ArticleTags = articleTags
                };

                articleDbContext.Articles.Add(article);
                articleDbContext.SaveChanges();

                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult DeleteArticle(string articleID)
        {
            Article article = articleDbContext.Articles.SingleOrDefault(i => i.ArticleCode == articleID);
            articleDbContext.Articles.Remove(article);
            articleDbContext.SaveChanges();

            RemoveNoArticleTags();
            return RedirectToAction("index");
        }

        public IActionResult ModifyArticle([FromForm]Article newArticle)
        {
            if (Request.Method == "POST")
            {
                //删除旧标签
                List<ArticleTag> oldTag = articleDbContext.ArticleTags.Where(i => i.ArticleId == newArticle.Id).ToList();
                oldTag.ForEach(i => articleDbContext.ArticleTags.Remove(i));

                List<ArticleTag> articleTags = new List<ArticleTag>();
                List<string> tags = new List<string>(Request.Form["tag"].ToString().TrimStart('#').Split("#"));
                tags.ForEach(i =>
                {
                    Tag tag = articleDbContext.Tags.SingleOrDefault(x => x.TagName == i);
                    if (tag == null)
                    {
                        articleDbContext.ArticleTags.Add(new ArticleTag
                        {
                            ArticleId = newArticle.Id,
                            Tag = new Tag() { TagName = i }
                        });
                    }
                    else
                    {
                        articleDbContext.ArticleTags.Add(new ArticleTag()
                        {
                            ArticleId = newArticle.Id,
                            TagId = tag.Id
                        });
                    }
                });

                articleDbContext.Articles.Update(newArticle);
                articleDbContext.SaveChanges();

                RemoveNoArticleTags();
                return RedirectToAction("index");
            }
            else
            {
                string articleID = Request.Query["articleID"];
                Article article = articleDbContext.Articles.Include(i => i.ArticleTags).ThenInclude(i => i.Tag).SingleOrDefault(i => i.ArticleCode == articleID);
                return View(article);
            }
        }

        public IActionResult Comment(string articleID)
        {
            ViewBag.ArticleID = articleID;
            List<Comment> comments = articleDbContext.Comments.Where(i => i.Article.ArticleCode == articleID).ToList();
            return View(comments);
        }

        public IActionResult DeleteComment(string articleID, int commentID)
        {
            Comment comment = articleDbContext.Comments.Single(i => i.Id == commentID);
            articleDbContext.Comments.Remove(comment);
            articleDbContext.SaveChanges();
            return Redirect("/Admin/Blog/Comment?articleID=" + articleID);
        }

        private void RemoveNoArticleTags()
        {
            //移除无博客的Tag
            List<Tag> noArticleTag = articleDbContext.Tags.Where(i => i.ArticleTags.Count == 0).ToList();
            noArticleTag.ForEach(i => articleDbContext.Tags.Remove(i));
            articleDbContext.SaveChanges();
        }

    }
}