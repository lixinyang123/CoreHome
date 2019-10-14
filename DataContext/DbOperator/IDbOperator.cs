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
        public void Delete(int id);

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="newModel">修改的对象</param>
        public void Modify(DbModel newModel);

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="id">查找的ID</param>
        /// <returns>查到的对象</returns>
        public DbModel Find(int id);

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="start">查找的起始索引</param>
        /// <param name="count">查找的数量</param>
        /// <returns></returns>
        public List<DbModel> Find(int start, int count);

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns>总数</returns>
        public int Count();
    }
}
