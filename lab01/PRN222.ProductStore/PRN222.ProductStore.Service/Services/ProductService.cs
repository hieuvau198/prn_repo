using AutoMapper;
using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Repository.Repositories.Interfaces;
using PRN222.ProductStore.Service.DTOs;
using PRN222.ProductStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IGenericRepository<Product> productRepository,
            IGenericRepository<Category> categoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetAsync(null, p => p.Category);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id, p => p.Category)
                          ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task AddProductAsync(CreateProductDto productDto)
        {
            if (string.IsNullOrWhiteSpace(productDto.ProductName))
                throw new ArgumentException("Product name is required.");

            if (productDto.CategoryId <= 0)
                throw new ArgumentException("Invalid category ID.");

            if (productDto.UnitPrice.HasValue && productDto.UnitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.");

            if (productDto.UnitsInStock.HasValue && productDto.UnitsInStock < 0)
                throw new ArgumentException("Units in stock cannot be negative.");

            var categoryExists = await _categoryRepository.FindAsync(c => c.CategoryId == productDto.CategoryId);
            if (categoryExists == null)
                throw new KeyNotFoundException($"Category with ID {productDto.CategoryId} not found.");

            var product = _mapper.Map<Product>(productDto);
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateProductAsync(UpdateProductDto productDto)
        {
            if (productDto.ProductId <= 0)
                throw new ArgumentException("Invalid product ID.");

            if (string.IsNullOrWhiteSpace(productDto.ProductName))
                throw new ArgumentException("Product name is required.");

            if (productDto.CategoryId <= 0)
                throw new ArgumentException("Invalid category ID.");

            if (productDto.UnitPrice.HasValue && productDto.UnitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.");

            if (productDto.UnitsInStock.HasValue && productDto.UnitsInStock < 0)
                throw new ArgumentException("Units in stock cannot be negative.");

            var existingProduct = await _productRepository.GetByIdAsync(productDto.ProductId)
                                   ?? throw new KeyNotFoundException($"Product with ID {productDto.ProductId} not found.");

            var categoryExists = await _categoryRepository.FindAsync(c => c.CategoryId == productDto.CategoryId);
            if (categoryExists == null)
                throw new KeyNotFoundException($"Category with ID {productDto.CategoryId} not found.");

            var updatedProduct = _mapper.Map(productDto, existingProduct);
            await _productRepository.UpdateAsync(updatedProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            var productExists = await _productRepository.FindAsync(p => p.ProductId == id);
            if (productExists == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            await _productRepository.DeleteAsync(id);
        }
    }
}
