using DataContext.DbConfig;
using DataContext.Models;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace DataContext.CacheOperator
{
    public class ArticleCacheOperator : ICacheOperator<Article>
    {
        public IDatabase database;

        public ArticleCacheOperator()
        {
            database = new DbConfigurator().CreateArticleCacheContext();
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="model">value</param>
        public void AddModel(string key, Article model)
        {
            string jsonStr = JsonSerializer.Serialize(model);
            database.StringSet(key, jsonStr);
        }

        /// <summary>
        /// 添加缓存列表
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="list">value</param>
        public void AddList(string key, List<Article> list)
        {
            list.ForEach(i =>
            {
                string jsonStr = JsonSerializer.Serialize(i);
                database.ListLeftPush(key, jsonStr);
            });
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public Article GetModel(string key)
        {
            string jsonStr = database.StringGet(key);
            return JsonSerializer.Deserialize<Article>(jsonStr);
        }

        /// <summary>
        /// 读取缓存列表
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public List<Article> GetList(string key)
        {
            RedisValue[] values = database.ListRange(key);
            List<RedisValue> list = new List<RedisValue>(values);
            List<Article> articles = new List<Article>();
            list.ForEach(i =>
            {
                articles.Add(JsonSerializer.Deserialize<Article>(i));
            });
            return articles.OrderByDescending(i => i.ID).ToList();
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">key</param>
        public void DelKey(string key)
        {
            database.KeyDelete(key);
        }
    }
}
