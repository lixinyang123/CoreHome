using CoreHome.Admin.ViewModels;
using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreHome.Admin.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public BlogController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index()
        {
            List<Article> articles = articleDbContext.Articles.ToList();
            return View(articles);
        }

        public IActionResult Upload()
        {
            ViewBag.Action = "Upload";
            return View("Editor", new ArticleViewModel());
        }

        [HttpPost]
        public IActionResult Upload(ArticleViewModel articleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Editor", articleViewModel);
            }

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

            articleDbContext.Articles.Add(new Article()
            {
                ArticleCode = Guid.NewGuid(),
                Title = articleViewModel.Title,
                Time = DateTime.Now,
                Category = category,
                ArticleTags = articleTags,
                Overview = articleViewModel.Overview,
                CoverUrl = articleViewModel.CoverUrl,
                Content = articleViewModel.Content
            });

            articleDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Modify(Guid id)
        {
            ViewBag.Action = "Modify";

            Article article = articleDbContext.Articles.Include(i=>i.Category)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .SingleOrDefault(i => i.ArticleCode == id);

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
                Title = article.Title,
                CategoryName = article.Category.CategoriesName,
                TagStr = tagStr.TrimEnd('#'),
                Overview = article.Overview,
                CoverUrl = article.CoverUrl,
                Content = article.Content
            };

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
            article.CoverUrl = articleViewModel.CoverUrl;
            article.Content = articleViewModel.Content;

            articleDbContext.SaveChanges();

            RemoveNoArticleCategory();
            RemoveNoArticleTags();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            Article article = articleDbContext.Articles.Include(i=>i.Comments)
                .Include(i=>i.ArticleTags)
                .SingleOrDefault(i => i.ArticleCode == id);

            if (article != null)
            {
                articleDbContext.Remove(article);
                article.ArticleTags.ForEach(i => articleDbContext.ArticleTags.Remove(i));
                article.Comments.ForEach(i => articleDbContext.Comments.Remove(i));
                articleDbContext.SaveChanges();

                RemoveNoArticleCategory();
                RemoveNoArticleTags();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Comment(Guid id)
        {
            Article article = articleDbContext.Articles
                .Include(i => i.Comments)
                .SingleOrDefault(i => i.ArticleCode == id);
            return View(article);
        }

        public IActionResult DelComment(int id)
        {
            Comment comment = articleDbContext.Comments.SingleOrDefault(i => i.Id == id);
            if (comment != null)
            {
                articleDbContext.Comments.Remove(comment);
                articleDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private void RemoveNoArticleCategory()
        {
            List<Category> noArticleCategories = articleDbContext.Categories.Where(i => i.Articles.Count == 0).ToList();
            noArticleCategories.ForEach(i => articleDbContext.Categories.Remove(i));
            articleDbContext.SaveChanges();
        }

        private void RemoveNoArticleTags()
        {
            List<Tag> noArticleTag = articleDbContext.Tags.Where(i => i.ArticleTags.Count == 0).ToList();
            noArticleTag.ForEach(i => articleDbContext.Tags.Remove(i));
            articleDbContext.SaveChanges();
        }

    }
}