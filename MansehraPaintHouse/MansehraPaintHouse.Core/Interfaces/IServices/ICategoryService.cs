using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByIdAsync(int id);
        Task<IQueryable<Category>> GetAllCategoriesQueryableAsync();
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<Category> CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task ToggleCategoryStatusAsync(int id);
        Task<Category> GetCategoryWithParentAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IQueryable<Category>> SearchCategoriesAsync(string searchTerm);
    }
} 