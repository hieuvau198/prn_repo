using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(string name, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Products.GetAsync(
                filter: p => string.IsNullOrEmpty(name) || p.ProductName.Contains(name),
                orderBy: q => q.OrderBy(p => p.ProductName),
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }

        public async Task<Product> GetByIdAsync(int id) => await _unitOfWork.Products.GetByIdAsync(id);

        public async Task AddAsync(Product product) => await _unitOfWork.Products.AddAsync(product);

        public async Task UpdateAsync(Product product) => await _unitOfWork.Products.UpdateAsync(product);

        public async Task DeleteAsync(int id) => await _unitOfWork.Products.DeleteAsync(id);
    }
}
