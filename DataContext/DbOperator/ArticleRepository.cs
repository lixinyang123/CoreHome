using DataContext.DbConfigurator;
using DataContext.ModelDbContext;
using DataContext.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataContext.DbOperator
{
    public class ArticleRepository
    {
        private readonly MysqlDbConfigurator configurator;

        public ArticleRepository()
        {
            configurator = new MysqlDbConfigurator();
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="article">文章对象</param>
        public void Add(Article article)
        {
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                context.Article.Add(article);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">文章id</param>
        public void Delete(int id)
        {
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                context.Article.Remove(Find(id));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 修改博客
        /// </summary>
        /// <param name="id">需要修改的博客ID</param>
        /// <param name="newArticle">修改后的博客</param>
        public void Modify(Article newArticle)
        {
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                //将新实体的修改进行插入
                context.Article.Update(newArticle);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 单个文章查找
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns>文章对象</returns>
        public Article Find(int id)
        {
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                return context.Article.Single(i => i.ID == id);
            }
        }

        /// <summary>
        /// 指定范围查找
        /// </summary>
        /// <param name="index">查找起点</param>
        /// <param name="pageSize">页面展示内容数量</param>
        /// <returns>文章对象列表</returns>
        public List<Article> Find(int index, int pageSize)
        {
            int limit = index * pageSize;
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                int count = Count() - limit;
                return context.Article.OrderByDescending(i => i.ID).Skip(limit).Take(count > 5 ? pageSize : count).ToList();
            }
        }

        /// <summary>
        /// 文章数量统计
        /// </summary>
        /// <returns>文章数量</returns>
        public int Count()
        {
            using (ArticleDbContext context = configurator.CreateArticleDbContext())
            {
                return context.Article.Count();
            }
        }

    }
}
