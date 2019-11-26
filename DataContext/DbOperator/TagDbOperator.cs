using DataContext.DbConfig;
using DataContext.ModelDbContext;
using DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataContext.DbOperator
{
    public class TagDbOperator : IDbOperator<Tag>
    {
        private readonly DbConfigurator configurator;

        public TagDbOperator()
        {
            configurator = new DbConfigurator();
        }

        /// <summary>
        /// 新增标签记录
        /// </summary>
        /// <param name="t">标签记录</param>
        public void Add(Tag t)
        {
            using ArticleDbContext dbContext = configurator.CreateArticleDbContext();
            dbContext.Tag.Add(t);
            dbContext.SaveChanges();
        }

        public int Count()
        {
            using ArticleDbContext dbContext = configurator.CreateArticleDbContext();
            return dbContext.Tag.Count();
        }

        /// <summary>
        /// 删除标签记录
        /// </summary>
        /// <param name="articleID"></param>
        public void Delete(string articleID)
        {
            using ArticleDbContext dbContext = configurator.CreateArticleDbContext();
            List<Tag> tags = dbContext.Tag.Where(i => i.ArticleID == articleID).ToList();
            for (int i = 0; i < tags.Count; i++)
            {
                dbContext.Remove(tags[i]);
            }
            dbContext.SaveChanges();
        }

        public Tag Find(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询博客标签
        /// </summary>
        /// <param name="func">查询条件</param>
        /// <param name="start">起始索引</param>
        /// <param name="count">查询范围</param>
        /// <returns>标签列表</returns>
        public List<Tag> Find(Func<Tag, bool> func, int start, int count)
        {
            using ArticleDbContext dbContext = configurator.CreateArticleDbContext();
            return dbContext.Tag.Where(func).OrderByDescending(i => i.ID).ToList();
        }

        public void Modify(Tag newModel)
        {
            throw new NotImplementedException();
        }
    }
}
