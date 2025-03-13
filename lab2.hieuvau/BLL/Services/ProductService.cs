﻿using Repositories.Entities;
using Repositories.Interfaces;
using Services.BusinessModels;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductModel?> GetByIdAsync(int productId)
        {
            Product? product = await _unitOfWork.Products.GetByIdAsync(productId, p => p.Category);
            return product != null ? MapToModel(product) : null;
        }

        public async Task<IEnumerable<ProductModel>> GetByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Products.GetAsync(p => p.CategoryId == categoryId, p => p.Category);
            return products.Select(MapToModel);
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAsync(null, p => p.Category);
            return products.Select(MapToModel);
        }

        public async Task CreateAsync(ProductModel model)
        {
            var product = MapToEntity(model);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(model.ProductId);
            if (product == null) return;

            product.ProductName = model.ProductName;
            product.CategoryId = model.CategoryId;
            product.UnitsInStock = model.UnitsInStock;
            product.UnitPrice = model.UnitPrice;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            await _unitOfWork.Products.DeleteAsync(productId);
            await _unitOfWork.SaveChangesAsync();
        }

        private static ProductModel MapToModel(Product entity)
        {
            return new ProductModel
            {
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                CategoryId = entity.CategoryId,
                UnitsInStock = entity.UnitsInStock,
                UnitPrice = entity.UnitPrice,
                CategoryName = entity.Category?.CategoryName
            };
        }

        private static Product MapToEntity(ProductModel model)
        {
            return new Product
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                CategoryId = model.CategoryId,
                UnitsInStock = model.UnitsInStock,
                UnitPrice = model.UnitPrice
            };
        }
    }
}
