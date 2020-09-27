using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> filter);

        Task<T> GetAsync(Expression<Func<T, bool>> filter);

        List<T> GetList(Expression<Func<T, bool>> filter = null);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter = null);

        int Add(T entity);

        Task<int> AddAsync(T entity);

        int AddRange(IList<T> entities);

        Task<int> AddRangeAsync(IList<T> entities);

        int Update(T entity);

        Task<int> UpdateAsync(T entity);

        int UpdateRange(IList<T> entities);

        Task<int> UpdateRangeAsync(IList<T> entities);

        int Delete(T entity);

        Task<int> DeleteAsync(T entity);

        int DeleteById(object entityId);

        Task<int> DeleteByIdAsync(object entityId);

        int DeleteRange(IList<T> entities);

        Task<int> DeleteRangeAsync(IList<T> entities);
    }
}