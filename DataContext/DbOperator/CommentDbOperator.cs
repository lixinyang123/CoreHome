using DataContext.DbConfig;
using DataContext.ModelDbContext;
using DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataContext.DbOperator
{
    public class CommentDbOperator : IDbOperator<Comment>
    {
        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="t"></param>
        public void Add(Comment t)
        {
            ArticleDbContext context = DbConfigurator.CreateArticleDbContext();
            context.Comment.Add(t);
            context.SaveChanges();
        }

        /// <summary>
        /// 统计评论总数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            ArticleDbContext context = DbConfigurator.CreateArticleDbContext();
            return context.Comment.Count();
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="commentID">评论ID</param>
        public void Delete(string commentID)
        {
            ArticleDbContext context = DbConfigurator.CreateArticleDbContext();
            context.Comment.Remove(Find(commentID));
            context.SaveChanges();
        }

        /// <summary>
        /// 查找评论
        /// </summary>
        /// <param name="commentID">评论ID</param>
        /// <returns>评论实体</returns>
        public Comment Find(string commentID)
        {
            ArticleDbContext context = DbConfigurator.CreateArticleDbContext();
            Comment comment = context.Comment.Single(i => i.CommentID == commentID);
            return comment;
        }

        /// <summary>
        /// 范围查找评论
        /// </summary>
        /// <param name="func">查询条件</param>
        /// <param name="start">起始索引</param>
        /// <param name="count">查询数量</param>
        /// <returns></returns>
        public List<Comment> Find(Func<Comment, bool> func, int start, int count)
        {
            ArticleDbContext context = DbConfigurator.CreateArticleDbContext();
            List<Comment> comments = context.Comment.Where(func).OrderByDescending(i => i.ID).ToList();
            return comments;
        }

        public void Modify(Comment newModel)
        {
            throw new NotImplementedException();
        }
    }
}
