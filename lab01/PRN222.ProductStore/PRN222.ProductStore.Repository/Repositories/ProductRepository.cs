using Microsoft.EntityFrameworkCore;
using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ProductStoreContext _productStoreContext;

        public ProductRepository(ProductStoreContext productStoreContext) : base(productStoreContext)
        {
            _productStoreContext = productStoreContext;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productStoreContext.Products.Include(c => c.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _productStoreContext.Products.Include(c => c.Category).ToListAsync();
        }
    }
}
