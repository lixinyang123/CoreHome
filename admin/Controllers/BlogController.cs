using System;
using System.IO;
using DataContext.DbOperator;
using DataContext.Models;
using Infrastructure.common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace admin.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleRepository articleRepository;
        private readonly int pageSize = 20;

        public BlogController()
        {
            articleRepository = new ArticleRepository();
        }

        public IActionResult Index(int index)
        {
            index = PageManager.GetStartIndex(index, articleRepository.Count(), pageSize);
            List<Article> articles = articleRepository.Find(index, pageSize);
            return View(articles);
        }

        public IActionResult UploadArticle()
        {
            if(Request.Method=="POST")
            {
                Article article = new Article();
                //标题
                article.Title = Request.Form["title"];
                //时间
                article.Time = DateTime.Now.ToString("yyyy/MM/dd");

                //封面
                IFormFile file = Request.Form.Files["cover"];
                //保存封面到本地
                using (Stream stream = file.OpenReadStream())
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                    string path = "C:\\Server\\BlogCover\\";
                    string coverName = Guid.NewGuid().ToString() + ".jpg";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    string fullPath = path + coverName;
                    System.IO.File.WriteAllBytes(fullPath, buffer);
                    article.Cover = fullPath;
                }

                //概述
                article.Overview = Request.Form["overview"];
                //内容
                article.Content = Request.Form["content"];

                articleRepository.Add(article);

                return Redirect("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult DelArticle(int id)
        {
            articleRepository.Delete(id);
            return Redirect("Index");
        }

    }
}