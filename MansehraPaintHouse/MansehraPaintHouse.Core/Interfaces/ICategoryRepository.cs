using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<Category> GetCategoryWithParentAsync(int id);
        Task<bool> IsCategoryActiveAsync(int id);
        Task ToggleCategoryStatusAsync(int id);
    }
} 