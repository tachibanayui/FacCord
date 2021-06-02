using IsekaiTechnologies.FacCord.Core.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public FacCordContext Context { get; protected set; }

        public Repository(FacCordContext ctx)
        {
            Context = ctx;
        }

        public async Task AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await Context.Set<T>().AddRangeAsync(entities);
        }

        public async IAsyncEnumerable<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            foreach (var item in Context.Set<T>().Where(predicate).ToList())
            {
                yield return item;
            } 
        }

        public IAsyncEnumerable<T> GetAllAsync()
        {
            return Context.Set<T>().AsAsyncEnumerable();
        }

        public async Task<T> GetAsync(long id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public Task RemoveAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }
    }
}
