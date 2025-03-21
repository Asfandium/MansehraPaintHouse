using Microsoft.EntityFrameworkCore;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using MansehraPaintHouse.Infrastructure.Data;

namespace MansehraPaintHouse.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<Category> GetCategoryWithParentAsync(int id)
        {
            return await _dbSet
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.CategoryID == id);
        }

        public async Task<bool> IsCategoryActiveAsync(int id)
        {
            var category = await GetByIdAsync(id);
            return category?.IsActive ?? false;
        }

        public async Task ToggleCategoryStatusAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                category.IsActive = !category.IsActive;
                Update(category);
            }
        }

        public IQueryable<Category> SearchCategories(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _dbSet.AsQueryable();
            }

            searchTerm = searchTerm.ToLower();
            return _dbSet.Where(c => 
                c.Name.ToLower().Contains(searchTerm) ||
                c.Description.ToLower().Contains(searchTerm) ||
                (c.ParentCategory != null && c.ParentCategory.Name.ToLower().Contains(searchTerm))
            );
        }
    }
} 