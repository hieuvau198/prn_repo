using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PRN222.Lab2.ProductStore.Repository.Models;

namespace PRN222.Lab2.ProductStore.Service.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null,
            int? pageIndex = null,
            int? pageSize = null);

        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
