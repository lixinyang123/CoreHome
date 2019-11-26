using DataContext.DbConfig;
using DataContext.Models;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Collections.Generic;

namespace DataContext.CacheOperator
{
    public class ArticleOperator : ICacheOperator<Article>
    {
        public IDatabase database;

        public ArticleOperator()
        {
            database = new DbConfigurator().CreateArticleCacheContext();
        }

        public void AddModel(string key, Article model)
        {
            string jsonStr = JsonSerializer.Serialize(model);
            database.StringSet(key, jsonStr);
        }

        public void AddList(string key, List<Article> list)
        {
            list.ForEach(i =>
            {
                string jsonStr = JsonSerializer.Serialize(i);
                database.ListLeftPush(key, jsonStr);
            });
        }

        public Article GetModel(string key)
        {
            string jsonStr = database.StringGet(key);
            return JsonSerializer.Deserialize<Article>(jsonStr);
        }

        public List<Article> GetList(string key)
        {
            RedisValue[] values = database.ListRange(key);
            List<RedisValue> list = new List<RedisValue>(values);
            List<Article> articles = new List<Article>();
            list.ForEach(i =>
            {
                articles.Add(JsonSerializer.Deserialize<Article>(i));
            });
            return articles;
        }
    }
}
