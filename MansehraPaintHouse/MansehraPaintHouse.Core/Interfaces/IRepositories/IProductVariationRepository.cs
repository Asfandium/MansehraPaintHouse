using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IRepositories
{
    public interface IProductVariationRepository : IGenericRepository<ProductVariation>
    {
        Task<IEnumerable<ProductVariation>> GetActiveProductVariationsAsync();
        Task<ProductVariation> GetProductVariationWithProductAsync(int id);
        Task<ProductVariation> GetProductVariationWithAttributesAsync(int id);
        Task<bool> IsProductVariationActiveAsync(int id);
        Task ToggleProductVariationStatusAsync(int id);
        IQueryable<ProductVariation> SearchProductVariations(string searchTerm);
        Task<IEnumerable<ProductVariation>> GetProductVariationsByProductAsync(int productId);
    }
} 