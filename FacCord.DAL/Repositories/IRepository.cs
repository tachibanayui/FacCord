using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Core.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(long id);
        IAsyncEnumerable<TEntity> GetAllAsync();
        IAsyncEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
