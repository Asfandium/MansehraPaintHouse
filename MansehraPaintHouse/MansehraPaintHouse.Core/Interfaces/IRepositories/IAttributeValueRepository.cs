using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IRepositories
{
    public interface IAttributeValueRepository : IGenericRepository<AttributeValue>
    {
        Task<IEnumerable<AttributeValue>> GetActiveAttributeValuesAsync();
        Task<AttributeValue> GetAttributeValueWithAttributeAsync(int id);
        Task<bool> IsAttributeValueActiveAsync(int id);
        Task ToggleAttributeValueStatusAsync(int id);
        IQueryable<AttributeValue> SearchAttributeValues(string searchTerm);
        Task<IEnumerable<AttributeValue>> GetAttributeValuesByAttributeAsync(int attributeId);
    }
} 