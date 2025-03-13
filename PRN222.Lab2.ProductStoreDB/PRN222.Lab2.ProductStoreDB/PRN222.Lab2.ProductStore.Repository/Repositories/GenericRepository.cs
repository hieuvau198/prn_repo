using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Repository.Repositories.Interface;
using System.Linq.Dynamic.Core;

namespace PRN222.Lab2.ProductStore.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MyStoreDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(MyStoreDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        int? pageIndex = null,
        int? pageSize = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Update(T entity)
        {
            await Task.Run(() => _dbSet.Update(entity));
        }

        public async Task Delete(T entity)
        {
            await Task.Run(() => _dbSet.Remove(entity));
        }
    }


}
