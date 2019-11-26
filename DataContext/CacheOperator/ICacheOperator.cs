using System.Collections.Generic;

namespace DataContext.CacheOperator
{
    public interface ICacheOperator<CacheModel>
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        public void AddModel(string key, CacheModel model);

        /// <summary>
        /// 添加缓存列表
        /// </summary>
        public void AddList(string key, List<CacheModel> list);

        /// <summary>
        /// 读取缓存
        /// </summary>
        public CacheModel GetModel(string key);

        /// <summary>
        /// 读取缓存列表
        /// </summary>
        public List<CacheModel> GetList(string key);
    }
}
