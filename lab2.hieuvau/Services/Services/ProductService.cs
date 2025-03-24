using Microsoft.AspNetCore.SignalR;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.BusinessModels;
using Services.Hubs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ProductHub> _productHub;

        public ProductService(IUnitOfWork unitOfWork, IHubContext<ProductHub> productHub)
        {
            _unitOfWork = unitOfWork;
            _productHub = productHub;
        }

        public async Task<ProductModel?> GetByIdAsync(int productId)
        {
            var includes = new Expression<Func<Product, object>>[] { p => p.Category };
            var product = await _unitOfWork.Products.GetByIdAsync(productId, includes);
            return product != null ? MapToModel(product) : null;
        }

        public async Task<IEnumerable<ProductModel>> GetByCategoryAsync(int categoryId)
        {
            var includes = new Expression<Func<Product, object>>[] { p => p.Category };
            var products = await _unitOfWork.Products.GetAsync(p => p.CategoryId == categoryId, includes);
            return products.Select(MapToModel);
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var includes = new Expression<Func<Product, object>>[] { p => p.Category };
            var products = await _unitOfWork.Products.GetAsync(null, includes);
            return products.Select(MapToModel);
        }

        public async Task<IEnumerable<ProductModel>> GetPagedAsync(int pageNumber = 1, int pageSize = 10)
        {
            var includes = new Expression<Func<Product, object>>[] { p => p.Category };
            var products = await _unitOfWork.Products.GetAsync(null, includes, pageNumber, pageSize);
            return products.Select(MapToModel);
        }

        public async Task CreateAsync(ProductModel model)
        {
            var product = MapToEntity(model);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            await _productHub.Clients.All.SendAsync("ReceiveProductUpdate");
            await _productHub.Clients.All.SendAsync("ReceiveNotication", $"{model.ProductName} was added!");
        }

        public async Task UpdateAsync(ProductModel model)
        {
            var existing = await _unitOfWork.Products.GetByIdAsync(model.ProductId);
            if (existing == null) return;

            existing.ProductName = model.ProductName;
            existing.CategoryId = model.CategoryId;
            existing.UnitsInStock = model.UnitsInStock;
            existing.UnitPrice = model.UnitPrice;

            await _unitOfWork.Products.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            await _productHub.Clients.All.SendAsync("ReceiveProductUpdate");
            await Task.Delay(300);
            await _productHub.Clients.All.SendAsync("ReceiveNotication", $"{model.ProductName} was updated!");
        }

        public async Task DeleteAsync(int productId)
        {
            var existing = await _unitOfWork.Products.GetByIdAsync(productId);
            if (existing == null) return;

            await _unitOfWork.Products.DeleteAsync(productId);
            await _unitOfWork.SaveChangesAsync();

            await _productHub.Clients.All.SendAsync("ReceiveProductUpdate");
            await _productHub.Clients.All.SendAsync("ReceiveNotication", $"{existing.ProductName} was deleted!");
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
