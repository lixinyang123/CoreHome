using CoreHome.Admin.Filter;
using CoreHome.Data.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoreHome.Admin.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class DatabaseController : Controller
    {
        private readonly ArticleDbContext articleDbContext;

        public DatabaseController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
        }

        public IActionResult Index()
        {
            if (articleDbContext.Database.CanConnect())
            {
                return Content("连接成功");
            }
            return Content("连接失败");
        }

        public IActionResult Migrate()
        {
            try
            {
                articleDbContext.Database.Migrate();
                return Content("迁移成功");
            }
            catch (Exception ex)
            {
                return Content("迁移失败\n" + ex);
            }
        }

        public IActionResult Create()
        {
            try
            {
                articleDbContext.Database.EnsureCreated();
                return Content("创建成功");
            }
            catch (Exception ex)
            {
                return Content("创建失败\n" + ex);
            }
        }

        public IActionResult Delete()
        {
            try
            {
                articleDbContext.Database.EnsureDeleted();
                return Content("删除成功");
            }
            catch (Exception ex)
            {
                return Content("创建失败\n" + ex);
            }
        }

    }
}