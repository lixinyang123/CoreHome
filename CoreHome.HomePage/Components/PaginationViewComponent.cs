using CoreHome.Data.DatabaseContext;
using CoreHome.HomePage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace CoreHome.HomePage.Components
{
    public class PaginationViewComponent : ViewComponent
    {
        private readonly ArticleDbContext articleDbContext;
        //每页包含的博客数量
        private readonly int pageSize;
        //分页栏可操作的页数
        private int maxLength = 5;

        public PaginationViewComponent(ArticleDbContext articleDbContext, IConfiguration configuration)
        {
            this.articleDbContext = articleDbContext;
            pageSize = configuration.GetValue<int>("PageSize");
        }

        public IViewComponentResult Invoke(int index)
        {
            //博客总页数
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(articleDbContext.Articles.Count()) / pageSize));
            //index = CorrectIndex(index, pageCount);
            if (pageCount < 5)
            {
                maxLength = pageCount;
            }
            var paginationViewModel = new PaginationViewModel()
            {
                CurrentIndex = index,
                PageCount = pageCount,
                MaxLength = maxLength
            };
            return View(paginationViewModel);
        }

        //矫正页码
        //页码<1时留在第一页
        //页码>总页数时留在最后一页
        private int CorrectIndex(int index, int pageCount)
        {
            if (index < 1)
            {
                index = 1;
            }
            if(index> pageCount)
            {
                index = pageCount;
            }
            return index;
        }
    }
}
