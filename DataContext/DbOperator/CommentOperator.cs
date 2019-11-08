using DataContext.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using DataContext.DbConfig;
using DataContext.ModelDbContext;

namespace DataContext.DbOperator
{
    public class CommentOperator : IDbOperator<Comment>
    {
        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="t"></param>
        public void Add(Comment t)
        {
            using ArticleDbContext context = new DbConfigurator().CreateArticleDbContext();
            context.Comment.Add(t);
            context.SaveChanges();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id">评论ID</param>
        public void Delete(string id)
        {
            using ArticleDbContext context = new DbConfigurator().CreateArticleDbContext();
            context.Comment.Remove(Find(id));
            context.SaveChanges();
        }

        /// <summary>
        /// 查找评论
        /// </summary>
        /// <param name="id">评论ID</param>
        /// <returns>评论实体</returns>
        public Comment Find(string id)
        {
            using ArticleDbContext context = new DbConfigurator().CreateArticleDbContext();
            Comment comment = context.Comment.Single(i => i.CommentID == id);
            return comment;
        }

        public List<Comment> Find(int start, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 按条件查找所有匹配结果
        /// </summary>
        /// <param name="func">查询条件</param>
        /// <returns>结果列表</returns>
        public List<Comment> FindAll(Func<Comment, bool> func)
        {
            using ArticleDbContext context = new DbConfigurator().CreateArticleDbContext();
            List<Comment> comments = context.Comment.Where(func).OrderByDescending(i=>i.ID).ToList();
            return comments;
        }
        
        public void Modify(Comment newModel)
        {
            throw new NotImplementedException();
        }
    }
}
