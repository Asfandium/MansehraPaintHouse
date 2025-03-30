using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface IProductAttributeService
    {
        Task<ProductAttribute> GetAttributeByIdAsync(int id);
        Task<IQueryable<ProductAttribute>> GetAllAttributesQueryableAsync();
        Task<IEnumerable<ProductAttribute>> GetActiveAttributesAsync();
        Task<ProductAttribute> CreateAttributeAsync(ProductAttribute attribute);
        Task UpdateAttributeAsync(ProductAttribute attribute);
        Task DeleteAttributeAsync(int id);
        Task ToggleAttributeStatusAsync(int id);
        Task<ProductAttribute> GetAttributeWithValuesAsync(int id);
        Task<IEnumerable<ProductAttribute>> GetAllAttributesAsync();
        Task<IQueryable<ProductAttribute>> SearchAttributesAsync(string searchTerm);
    }
} 