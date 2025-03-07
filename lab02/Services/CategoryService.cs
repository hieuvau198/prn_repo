using BusinessObjects;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<Category> Categories, int TotalCount)> SearchCategoriesAsync(string name, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Categories.GetAsync(
                filter: c => string.IsNullOrEmpty(name) || c.CategoryName.Contains(name),
                orderBy: q => q.OrderBy(c => c.CategoryName),
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }

        public async Task<Category> GetByIdAsync(int id) => await _unitOfWork.Categories.GetByIdAsync(id);

        public async Task AddAsync(Category category) => await _unitOfWork.Categories.AddAsync(category);

        public async Task UpdateAsync(Category category) => await _unitOfWork.Categories.UpdateAsync(category);

        public async Task DeleteAsync(int id) => await _unitOfWork.Categories.DeleteAsync(id);
    }
}
