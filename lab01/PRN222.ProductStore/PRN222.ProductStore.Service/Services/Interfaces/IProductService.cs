using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Service.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Services.Interfaces
{
    public interface IProductService
    {
        Task SaveProduct(ProductModel p);
        Task UpdateProduct(ProductModel p);
        Task DeleteProduct(Product p);
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(int id);
    }
}
