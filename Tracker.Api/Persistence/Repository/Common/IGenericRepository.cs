using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tracker.Api.Models.Common;
using Tracker.Api.Persistence.Repository.Common;

namespace Tracker.Api.Persistence.Repository.Common
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IEnumerable<T> All();
        T FindById(int id);

        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(int id, T entity);
        Task<T> DeleteAsync(int id);

        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindByInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        //AllIncluding(c => c.Orders, c.ContactDetail);
        //FindByInclude(c => c.LastName.StartWith("L") && c.DateOfBirth >= date, c => c.Orders)
    }
}