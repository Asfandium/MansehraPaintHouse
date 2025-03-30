using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MansehraPaintHouse.Infrastructure.Services
{
    public class AttributeValueService : IAttributeValueService
    {
        private readonly IAttributeValueRepository _attributeValueRepository;

        public AttributeValueService(IAttributeValueRepository attributeValueRepository)
        {
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<AttributeValue> GetAttributeValueByIdAsync(int id)
        {
            return await _attributeValueRepository.GetByIdAsync(id);
        }

        public async Task<IQueryable<AttributeValue>> GetAllAttributeValuesQueryableAsync()
        {
            return _attributeValueRepository.GetQueryable().Include(av => av.ProductAttribute);
        }

        public async Task<IEnumerable<AttributeValue>> GetActiveAttributeValuesAsync()
        {
            return await _attributeValueRepository.GetActiveAttributeValuesAsync();
        }

        public async Task<AttributeValue> CreateAttributeValueAsync(AttributeValue attributeValue)
        {
            await _attributeValueRepository.AddAsync(attributeValue);
            return attributeValue;
        }

        public async Task UpdateAttributeValueAsync(AttributeValue attributeValue)
        {
            _attributeValueRepository.Update(attributeValue);
        }

        public async Task DeleteAttributeValueAsync(int id)
        {
            var attributeValue = await GetAttributeValueByIdAsync(id);
            if (attributeValue != null)
            {
                _attributeValueRepository.Remove(attributeValue);
            }
        }

        public async Task ToggleAttributeValueStatusAsync(int id)
        {
            await _attributeValueRepository.ToggleAttributeValueStatusAsync(id);
        }

        public async Task<AttributeValue> GetAttributeValueWithAttributeAsync(int id)
        {
            return await _attributeValueRepository.GetAttributeValueWithAttributeAsync(id);
        }

        public async Task<IEnumerable<AttributeValue>> GetAllAttributeValuesAsync()
        {
            return await _attributeValueRepository.GetAllAsync();
        }

        public async Task<IQueryable<AttributeValue>> SearchAttributeValuesAsync(string searchTerm)
        {
            return _attributeValueRepository.SearchAttributeValues(searchTerm);
        }

        public async Task<IEnumerable<AttributeValue>> GetAttributeValuesByAttributeAsync(int attributeId)
        {
            return await _attributeValueRepository.GetAttributeValuesByAttributeAsync(attributeId);
        }
    }
} 