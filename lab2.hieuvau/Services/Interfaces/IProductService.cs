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
        Task<IEnumerable<ProductModel>> GetPagedAsync(int pageNumber = 1, int pageSize = 10);
        Task CreateAsync(ProductModel model);
        Task UpdateAsync(ProductModel model);
        Task DeleteAsync(int productId);
    }
}
