using BusinessObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ICategoryService
    {
        Task<(IEnumerable<Category> Categories, int TotalCount)> SearchCategoriesAsync(string name, int pageIndex, int pageSize);
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
