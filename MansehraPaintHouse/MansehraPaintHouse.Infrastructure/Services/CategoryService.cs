using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using MansehraPaintHouse.Core.Interfaces.IRepositories;

namespace MansehraPaintHouse.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IQueryable<Category>> GetAllCategoriesQueryableAsync()
        {
            return _categoryRepository.GetQueryable();
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _categoryRepository.GetActiveCategoriesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _categoryRepository.Update(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
            {
                _categoryRepository.Remove(category);
            }
        }

        public async Task ToggleCategoryStatusAsync(int id)
        {
            await _categoryRepository.ToggleCategoryStatusAsync(id);
        }

        public async Task<Category> GetCategoryWithParentAsync(int id)
        {
            return await _categoryRepository.GetCategoryWithParentAsync(id);
        }

        public async Task<IQueryable<Category>> SearchCategoriesAsync(string searchTerm)
        {
            return _categoryRepository.SearchCategories(searchTerm);
        }
    }
} 