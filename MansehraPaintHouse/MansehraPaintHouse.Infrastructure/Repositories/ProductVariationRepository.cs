using Microsoft.EntityFrameworkCore;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using MansehraPaintHouse.Infrastructure.Data;

namespace MansehraPaintHouse.Infrastructure.Repositories
{
    public class ProductVariationRepository : GenericRepository<ProductVariation>, IProductVariationRepository
    {
        public ProductVariationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductVariation>> GetActiveProductVariationsAsync()
        {
            return await _dbSet.Where(pv => pv.IsActive).ToListAsync();
        }

        public async Task<ProductVariation> GetProductVariationWithProductAsync(int id)
        {
            return await _dbSet
                .Include(pv => pv.Product)
                .FirstOrDefaultAsync(pv => pv.VariationID == id);
        }

        public async Task<ProductVariation> GetProductVariationWithAttributesAsync(int id)
        {
            return await _dbSet
                .Include(pv => pv.ProductVariationAttributes)
                    .ThenInclude(pva => pva.Attribute)
                .Include(pv => pv.ProductVariationAttributes)
                    .ThenInclude(pva => pva.AttributeValue)
                .FirstOrDefaultAsync(pv => pv.VariationID == id);
        }

        public async Task<bool> IsProductVariationActiveAsync(int id)
        {
            var productVariation = await GetByIdAsync(id);
            return productVariation?.IsActive ?? false;
        }

        public async Task ToggleProductVariationStatusAsync(int id)
        {
            var productVariation = await GetByIdAsync(id);
            if (productVariation != null)
            {
                productVariation.IsActive = !productVariation.IsActive;
                Update(productVariation);
            }
        }

        public IQueryable<ProductVariation> SearchProductVariations(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _dbSet.AsQueryable();
            }

            searchTerm = searchTerm.ToLower();
            return _dbSet.Where(pv =>
                pv.Description.ToLower().Contains(searchTerm) ||
                pv.SKU.ToLower().Contains(searchTerm));
        }

        public async Task<IEnumerable<ProductVariation>> GetProductVariationsByProductAsync(int productId)
        {
            return await _dbSet.Where(pv => pv.ProductID == productId).ToListAsync();
        }
    }
} 