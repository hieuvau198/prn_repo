using Microsoft.AspNetCore.SignalR;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.BusinessModels;
using Services.Hubs;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ProductHub> _productHub; // Inject SignalR Hub

        public ProductService(IUnitOfWork unitOfWork, IHubContext<ProductHub> productHub)
        {
            _unitOfWork = unitOfWork;
            _productHub = productHub;
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

            // 🔔 Notify clients that a new product was added
            await _productHub.Clients.All.SendAsync("ReceiveProductUpdate");
            await _productHub.Clients.All.SendAsync("ReceiveNotication", $"{model.ProductName} was added!");
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

            // 🔔 Notify clients that a product was updated
            await _productHub.Clients.All.SendAsync("ReceiveProductUpdate");
            await Task.Delay(300);
            await _productHub.Clients.All.SendAsync("ReceiveNotication", $"{model.ProductName} was updated!");

        }

        public async Task DeleteAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null) return;

            await _unitOfWork.Products.DeleteAsync(productId);
            await _unitOfWork.SaveChangesAsync();


            // 🔔 Notify clients that a product was deleted
            await _productHub.Clients.All.SendAsync("ReceiveProductUpdate");
            await _productHub.Clients.All.SendAsync("ReceiveNotication", $"{product.ProductName} was deleted!");

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
