using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Lab2.ProductStore.Repository.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        int? pageIndex = null,
        int? pageSize = null);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }

}
