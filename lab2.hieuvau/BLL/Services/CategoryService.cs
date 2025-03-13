using BLL.BusinessModels;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAsync();
            return categories.Select(MapToModel);
        }

        public async Task<CategoryModel?> GetByIdAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            return category != null ? MapToModel(category) : null;
        }

        public async Task CreateAsync(CategoryModel model)
        {
            var category = MapToEntity(model);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryModel model)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(model.CategoryId);
            if (category == null) return;

            category.CategoryName = model.CategoryName;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int categoryId)
        {
            await _unitOfWork.Categories.DeleteAsync(categoryId);
            await _unitOfWork.SaveChangesAsync();
        }

        private static CategoryModel MapToModel(Category entity)
        {
            return new CategoryModel
            {
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName
            };
        }

        private static Category MapToEntity(CategoryModel model)
        {
            return new Category
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName
            };
        }
    }
}
