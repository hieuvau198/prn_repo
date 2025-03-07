using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Repository.Repositories;
using PRN222.ProductStore.Repository.Repositories.Interfaces;
using PRN222.ProductStore.Service.BussinessModels;
using PRN222.ProductStore.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task DeleteProduct(Product p)
        {
            await _productRepository.DeleteAsync(p.ProductId);
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _productRepository.GetProducts();
        }

        public async Task SaveProduct(ProductModel p)
        {
            var product = new Product
            {
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                UnitsInStock = p.UnitsInStock,
                UnitPrice = p.UnitPrice
            };

            await _productRepository.AddAsync(product);
        }

        public async Task UpdateProduct(ProductModel p)
        {
            var product = new Product
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                UnitsInStock = p.UnitsInStock,
                UnitPrice = p.UnitPrice
            };

            await _productRepository.UpdateAsync(product);
        }
    }
}
