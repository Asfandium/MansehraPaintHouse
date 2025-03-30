using Microsoft.EntityFrameworkCore;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using MansehraPaintHouse.Infrastructure.Data;

namespace MansehraPaintHouse.Infrastructure.Repositories
{
    public class AttributeValueRepository : GenericRepository<AttributeValue>, IAttributeValueRepository
    {
        public AttributeValueRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AttributeValue>> GetActiveAttributeValuesAsync()
        {
            return await _dbSet.Where(av => av.IsActive).ToListAsync();
        }

        public async Task<AttributeValue> GetAttributeValueWithAttributeAsync(int id)
        {
            return await _dbSet
                .Include(av => av.ProductAttribute)
                .FirstOrDefaultAsync(av => av.AttributeValueID == id);
        }

        public async Task<bool> IsAttributeValueActiveAsync(int id)
        {
            var attributeValue = await GetByIdAsync(id);
            return attributeValue?.IsActive ?? false;
        }

        public async Task ToggleAttributeValueStatusAsync(int id)
        {
            var attributeValue = await GetByIdAsync(id);
            if (attributeValue != null)
            {
                attributeValue.IsActive = !attributeValue.IsActive;
                Update(attributeValue);
            }
        }

        public IQueryable<AttributeValue> SearchAttributeValues(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _dbSet.AsQueryable();
            }

            searchTerm = searchTerm.ToLower();
            return _dbSet.Where(av =>
                av.Value.ToLower().Contains(searchTerm) ||
                av.Code.ToLower().Contains(searchTerm));
        }

        public async Task<IEnumerable<AttributeValue>> GetAttributeValuesByAttributeAsync(int attributeId)
        {
            return await _dbSet.Where(av => av.ProductAttributeID == attributeId).ToListAsync();
        }
    }
} 