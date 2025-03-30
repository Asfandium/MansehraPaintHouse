using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface IProductVariationService
    {
        Task<ProductVariation> GetProductVariationByIdAsync(int id);
        Task<IQueryable<ProductVariation>> GetAllProductVariationsQueryableAsync();
        Task<IEnumerable<ProductVariation>> GetActiveProductVariationsAsync();
        Task<ProductVariation> CreateProductVariationAsync(ProductVariation productVariation);
        Task UpdateProductVariationAsync(ProductVariation productVariation);
        Task DeleteProductVariationAsync(int id);
        Task ToggleProductVariationStatusAsync(int id);
        Task<ProductVariation> GetProductVariationWithProductAsync(int id);
        Task<ProductVariation> GetProductVariationWithAttributesAsync(int id);
        Task<IEnumerable<ProductVariation>> GetAllProductVariationsAsync();
        Task<IQueryable<ProductVariation>> SearchProductVariationsAsync(string searchTerm);
        Task<IEnumerable<ProductVariation>> GetProductVariationsByProductAsync(int productId);
    }
} 