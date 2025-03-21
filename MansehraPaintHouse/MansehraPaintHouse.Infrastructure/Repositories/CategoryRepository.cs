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
                // Search by Name or Description
                c.Name.ToLower().Contains(searchTerm) ||
                c.Description.ToLower().Contains(searchTerm) ||

                // Partial match for Active/Inactive status
                //(c.IsActive && "active".Contains(searchTerm)) ||
                //(!c.IsActive && "inactive".Contains(searchTerm)) ||

                // Exact match for Active/Inactive status
                (searchTerm == "active" && c.IsActive) ||
                (searchTerm == "inactive" && !c.IsActive) ||



                // Partial match for Master Category
                (c.ParentCategory == null && "master category".Contains(searchTerm)) ||
                (c.ParentCategory != null && c.ParentCategory.Name.ToLower().Contains(searchTerm))
            );
        }
    }
} 