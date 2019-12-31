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
        /// <summary>
        /// 新增标签记录
        /// </summary>
        /// <param name="t">标签记录</param>
        public void Add(Tag t)
        {
            using ArticleDbContext context = DbConfigurator.GetDbContext();
            context.Tag.Add(t);
            context.SaveChanges();
        }

        public int Count()
        {
            using ArticleDbContext context = DbConfigurator.GetDbContext();
            return context.Tag.Count();
        }

        /// <summary>
        /// 删除标签记录
        /// </summary>
        /// <param name="articleID"></param>
        public void Delete(string articleID)
        {
            using ArticleDbContext context = DbConfigurator.GetDbContext();
            List<Tag> tags = context.Tag.Where(i => i.ArticleID == articleID).ToList();
            for (int i = 0; i < tags.Count; i++)
            {
                context.Remove(tags[i]);
            }
            context.SaveChanges();
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
            using ArticleDbContext context = DbConfigurator.GetDbContext();
            return context.Tag.Where(func).OrderByDescending(i => i.ID).ToList();
        }

        public void Modify(Tag newModel)
        {
            throw new NotImplementedException();
        }
    }
}
