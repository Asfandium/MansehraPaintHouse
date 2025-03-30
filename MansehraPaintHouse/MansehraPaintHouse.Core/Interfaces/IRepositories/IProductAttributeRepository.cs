using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IRepositories
{
    public interface IProductAttributeRepository : IGenericRepository<ProductAttribute>
    {
        Task<IEnumerable<ProductAttribute>> GetActiveAttributesAsync();
        Task<ProductAttribute> GetAttributeWithValuesAsync(int id);
        Task<bool> IsAttributeActiveAsync(int id);
        Task ToggleAttributeStatusAsync(int id);
        IQueryable<ProductAttribute> SearchAttributes(string searchTerm);
    }
} 