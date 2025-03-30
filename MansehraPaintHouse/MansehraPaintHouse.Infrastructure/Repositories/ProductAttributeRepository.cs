//using Microsoft.EntityFrameworkCore;
//using MansehraPaintHouse.Core.Entities;
//using MansehraPaintHouse.Core.Interfaces.IRepositories;
//using MansehraPaintHouse.Infrastructure.Data;

//namespace MansehraPaintHouse.Infrastructure.Repositories
//{
//    public class ProductAttributeRepository : GenericRepository<ProductAttribute>, IProductAttributeRepository
//    {
//        public ProductAttributeRepository(ApplicationDbContext context) : base(context)
//        {
//        }

//        public async Task<IEnumerable<ProductAttribute>> GetActiveAttributesAsync()
//        {
//            return await _dbSet.Where(a => a.IsActive).ToListAsync();
//        }

//        public async Task<ProductAttribute> GetAttributeWithValuesAsync(int id)
//        {
//            return await _dbSet
//                .Include(a => a.AttributeValues)
//                .FirstOrDefaultAsync(a => a.ProductAttributeID == id);
//        }

//        public async Task<bool> IsAttributeActiveAsync(int id)
//        {
//            var attribute = await GetByIdAsync(id);
//            return attribute?.IsActive ?? false;
//        }

//        public async Task ToggleAttributeStatusAsync(int id)
//        {
//            var attribute = await GetByIdAsync(id);
//            if (attribute != null)
//            {
//                attribute.IsActive = !attribute.IsActive;
//                Update(attribute);
//            }
//        }

//        public IQueryable<ProductAttribute> SearchAttributes(string searchTerm)
//        {
//            if (string.IsNullOrWhiteSpace(searchTerm))
//            {
//                return _dbSet.AsQueryable();
//            }

//            searchTerm = searchTerm.ToLower();
//            return _dbSet.Where(a =>
//                a.Name.ToLower().Contains(searchTerm));
//        }
//    }
//} 


using Microsoft.EntityFrameworkCore;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using MansehraPaintHouse.Infrastructure.Data;

namespace MansehraPaintHouse.Infrastructure.Repositories
{
    public class ProductAttributeRepository : GenericRepository<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductAttribute>> GetActiveAttributesAsync()
        {
            return await _dbSet.Where(a => a.IsActive).ToListAsync();
        }

        public async Task<ProductAttribute> GetAttributeWithValuesAsync(int id)
        {
            return await _dbSet
                .Include(a => a.AttributeValues)
                .FirstOrDefaultAsync(a => a.ProductAttributeID == id);
        }

        public async Task<bool> IsAttributeActiveAsync(int id)
        {
            var attribute = await GetByIdAsync(id);
            return attribute?.IsActive ?? false;
        }

        public async Task ToggleAttributeStatusAsync(int id)
        {
            var attribute = await GetByIdAsync(id);
            if (attribute != null)
            {
                attribute.IsActive = !attribute.IsActive;
                Update(attribute);
            }
        }

        public IQueryable<ProductAttribute> SearchAttributes(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _dbSet.AsQueryable();
            }

            searchTerm = searchTerm.ToLower();
            return _dbSet.Where(a => a.Name.ToLower().Contains(searchTerm));
        }
    }
}