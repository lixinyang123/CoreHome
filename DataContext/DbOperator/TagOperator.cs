using DataContext.DbConfig;
using DataContext.ModelDbContext;
using DataContext.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DataContext.DbOperator
{
    public class TagOperator : IDbOperator<Tag>
    {
        private readonly DbConfigurator dbConfigurator;

        public TagOperator()
        {
            dbConfigurator = new DbConfigurator();
        }

        public void Add(Tag t)
        {
            using ArticleDbContext dbContext = dbConfigurator.CreateArticleDbContext();
            dbContext.Tag.Add(t);
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Tag Find(string id)
        {
            throw new NotImplementedException();
        }

        public List<Tag> Find(Func<Tag, bool> func, int start, int count)
        {
            throw new NotImplementedException();
        }

        public void Modify(Tag newModel)
        {
            throw new NotImplementedException();
        }
    }
}
