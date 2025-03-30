using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface IAttributeValueService
    {
        Task<AttributeValue> GetAttributeValueByIdAsync(int id);
        Task<IQueryable<AttributeValue>> GetAllAttributeValuesQueryableAsync();
        Task<IEnumerable<AttributeValue>> GetActiveAttributeValuesAsync();
        Task<AttributeValue> CreateAttributeValueAsync(AttributeValue attributeValue);
        Task UpdateAttributeValueAsync(AttributeValue attributeValue);
        Task DeleteAttributeValueAsync(int id);
        Task ToggleAttributeValueStatusAsync(int id);
        Task<AttributeValue> GetAttributeValueWithAttributeAsync(int id);
        Task<IEnumerable<AttributeValue>> GetAllAttributeValuesAsync();
        Task<IQueryable<AttributeValue>> SearchAttributeValuesAsync(string searchTerm);
        Task<IEnumerable<AttributeValue>> GetAttributeValuesByAttributeAsync(int attributeId);
    }
} 