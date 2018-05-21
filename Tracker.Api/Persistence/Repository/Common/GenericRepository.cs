using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tracker.Api.Models.Common;

namespace Tracker.Api.Persistence.Repository.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> _dbset;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            _dbset = context.Set<T>();
        }

        public IEnumerable<T> All()
        {
            return _dbset.AsNoTracking().ToList();
        }

        public T FindById(int id)
        {
            return _dbset.AsNoTracking().SingleOrDefault(e => e.Id == id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            var oldEntity = FindById(id);
            if (oldEntity == null) return oldEntity;

            _dbset.Attach(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> DeleteAsync(int id)
        {

            var request = _dbset.FirstOrDefault(x => x.Id == id);
            if (request == null) return request;
            _dbset.Remove(request);
            await context.SaveChangesAsync();

            return request; //request;
        }


        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> results = _dbset.AsNoTracking().Where(predicate).ToList();
            return results;
        }

        public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbset;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsEnumerable();
        }

        public IEnumerable<T> FindByInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            IEnumerable<T> results = query.Where(predicate).ToList();
            return results;
        }


        ///
        private IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = _dbset.AsNoTracking();
            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        }



    }
}