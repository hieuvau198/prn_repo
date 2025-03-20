using Services.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync();
        Task<CategoryModel?> GetByIdAsync(int categoryId);
        Task CreateAsync(CategoryModel model);
        Task UpdateAsync(CategoryModel model);
        Task DeleteAsync(int categoryId);
    }
}
