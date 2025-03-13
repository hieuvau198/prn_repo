using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PRN222.Lab2.ProductStore.Repository.Models;

namespace PRN222.Lab2.ProductStore.Service.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(
            Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null,
            int? pageIndex = null,
            int? pageSize = null);

        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
