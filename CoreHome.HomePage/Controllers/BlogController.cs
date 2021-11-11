using CoreHome.Data.DatabaseContext;
using CoreHome.Data.Models;
using CoreHome.HomePage.ViewModels;
using CoreHome.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                .OrderByDescending(i => i.Id)
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
                int count = articleDbContext.Articles.Where(i => i.Title.ToLower().Contains(id.ToLower())).Count();
                return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / pageSize));
            });
            index = CorrectIndex(index, pageCount);

            List<Article> articles = await articleDbContext.Articles
                .OrderByDescending(i => i.Id)
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Where(i => i.Title.ToLower().Contains(id.ToLower()) || i.Overview.ToLower().Contains(id.ToLower()))
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