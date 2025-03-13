using Services.BusinessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductModel?> GetByIdAsync(int productId);
        Task<IEnumerable<ProductModel>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductModel>> GetAllAsync();
        Task CreateAsync(ProductModel model);
        Task UpdateAsync(ProductModel model);
        Task DeleteAsync(int productId);
    }
}
