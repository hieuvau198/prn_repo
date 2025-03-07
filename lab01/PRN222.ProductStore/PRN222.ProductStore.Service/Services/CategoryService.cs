using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Repository.Repositories;
using PRN222.ProductStore.Repository.Repositories.Interfaces;
using PRN222.ProductStore.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _genericRepository;

        public CategoryService(IGenericRepository<Category> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _genericRepository.GetAllAsync() as List<Category>;
        }
    }
}
