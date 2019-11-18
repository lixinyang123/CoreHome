using System;
using System.Collections.Generic;

namespace DataContext.DbOperator
{
    public interface IDbOperator<DbModel>
    {
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="t">新增对象</param>
        public void Add(DbModel t);

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="id">删除对象的ID</param>
        public void Delete(string id);

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="newModel">修改的对象</param>
        public void Modify(DbModel newModel);

        /// <summary>
        /// 单个查找
        /// </summary>
        /// <param name="id">查找的ID</param>
        /// <returns>查到的对象</returns>
        public DbModel Find(string id);

       /// <summary>
       /// 范围查找
       /// </summary>
       /// <param name="func">查询条件</param>
       /// <param name="start">起始索引</param>
       /// <param name="count">查询数量</param>
       /// <returns></returns>
        public List<DbModel> Find(Func<DbModel, bool> func, int start, int count);

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns>总数</returns>
        public int Count();
    }
}
