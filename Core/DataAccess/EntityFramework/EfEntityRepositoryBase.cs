using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public int Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Added;
                return context.SaveChanges();
            }
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Added;
                return await context.SaveChangesAsync();
            }
        }

        public int AddRange(IList<TEntity> entities)
        {
            using (var context = new TContext())
            {
                context.AddRange(entities);
                return context.SaveChanges();
            }
        }

        public async Task<int> AddRangeAsync(IList<TEntity> entities)
        {
            using (var context = new TContext())
            {
                await context.AddRangeAsync(entities);
                return await context.SaveChangesAsync();
            }
        }

        public int Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                return context.SaveChanges();
            }
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                return await context.SaveChangesAsync();
            }
        }

        public int DeleteById(object entityId)
        {
            using (var context = new TContext())
            {
                var entityToDelete = context.Set<TEntity>().Find(entityId);
                var deletedEntity = context.Entry(entityToDelete);
                deletedEntity.State = EntityState.Deleted;
                return context.SaveChanges();
            }
        }

        public async Task<int> DeleteByIdAsync(object entityId)
        {
            using (var context = new TContext())
            {
                var entityToDelete = context.Set<TEntity>().Find(entityId);
                var deletedEntity = context.Entry(entityToDelete);
                deletedEntity.State = EntityState.Deleted;
                return await context.SaveChangesAsync();
            }
        }

        public int DeleteRange(IList<TEntity> entities)
        {
            using (var context = new TContext())
            {
                context.RemoveRange(entities);
                return context.SaveChanges();
            }
        }

        public async Task<int> DeleteRangeAsync(IList<TEntity> entities)
        {
            using (var context = new TContext())
            {
                context.RemoveRange(entities);
                return await context.SaveChangesAsync();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().SingleOrDefaultAsync(filter);
            }
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null ? await context.Set<TEntity>().ToListAsync() : await context.Set<TEntity>().Where(filter).ToListAsync();
            }
        }

        public int Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                return context.SaveChanges();
            }
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                return await context.SaveChangesAsync();
            }
        }

        public int UpdateRange(IList<TEntity> entities)
        {
            using (var context = new TContext())
            {
                context.UpdateRange(entities);
                return context.SaveChanges();
            }
        }

        public async Task<int> UpdateRangeAsync(IList<TEntity> entities)
        {
            using (var context = new TContext())
            {
                context.UpdateRange(entities);
                return await context.SaveChangesAsync();
            }
        }
    }
}