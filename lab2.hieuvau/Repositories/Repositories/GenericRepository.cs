using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MyStoreContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(MyStoreContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAsync(
    Expression<Func<T, bool>> predicate = null,
    Expression<Func<T, object>>[] includes = null,
    int? pageNumber = null,
    int? pageSize = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (pageNumber.HasValue || pageSize.HasValue)
            {
                int page = pageNumber ?? 1;
                int size = pageSize ?? 10;

                query = query.Skip((page - 1) * size).Take(size);
            }

            return await query.ToListAsync();
        }



        public async Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Apply Includes if provided
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Get the key name dynamically instead of assuming "Id"
            var keyProperty = _context.Model.FindEntityType(typeof(T))
                                           .FindPrimaryKey()
                                           .Properties
                                           .FirstOrDefault()?.Name;

            if (keyProperty == null)
            {
                throw new InvalidOperationException("Entity has no primary key defined.");
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, keyProperty).Equals(id));
        }


        public async Task<T> FindAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            // Apply Includes if provided
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
