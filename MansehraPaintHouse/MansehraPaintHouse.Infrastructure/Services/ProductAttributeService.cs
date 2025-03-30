//using MansehraPaintHouse.Core.Entities;
//using MansehraPaintHouse.Core.Interfaces.IServices;
//using MansehraPaintHouse.Core.Interfaces.IRepositories;
//using Microsoft.EntityFrameworkCore;
//using MansehraPaintHouse.Infrastructure.Repositories;

//namespace MansehraPaintHouse.Infrastructure.Services
//{
//    public class ProductAttributeService : IProductAttributeService
//    {
//        private readonly IProductAttributeRepository _productattributeRepository;

//        public ProductAttributeService(IProductAttributeRepository productattributeRepository)
//        {
//            _productattributeRepository = productattributeRepository;
//        }

//        public async Task<ProductAttribute> GetAttributeByIdAsync(int id)
//        {
//            return await _productattributeRepository.GetByIdAsync(id);
//        }

//        public async Task<IQueryable<ProductAttribute>> GetAllAttributesQueryableAsync()
//        {
//            return _productattributeRepository.GetQueryable().Include(a => a.AttributeValues);
//        }

//        public async Task<IEnumerable<Attribute>> GetActiveAttributesAsync()
//        {
//            return await _productattributeRepository.GetActiveAttributesAsync();
//        }

//        public async Task<Attribute> CreateAttributeAsync(Attribute attribute)
//        {
//            await _productattributeRepository.AddAsync(attribute);
//            return attribute;
//        }

//        public async Task UpdateAttributeAsync(Attribute attribute)
//        {
//            _productattributeRepository.Update(attribute);
//        }

//        public async Task DeleteAttributeAsync(int id)
//        {
//            var attribute = await GetAttributeByIdAsync(id);
//            if (attribute != null)
//            {
//                _productattributeRepository.Remove(attribute);
//            }
//        }

//        public async Task ToggleAttributeStatusAsync(int id)
//        {
//            await _productattributeRepository.ToggleAttributeStatusAsync(id);
//        }

//        public async Task<Attribute> GetAttributeWithValuesAsync(int id)
//        {
//            return await _productattributeRepository.GetAttributeWithValuesAsync(id);
//        }

//        public async Task<IEnumerable<Attribute>> GetAllAttributesAsync()
//        {
//            return await _productattributeRepository.GetAllAsync();
//        }

//        public async Task<IQueryable<Attribute>> SearchAttributesAsync(string searchTerm)
//        {
//            return _productattributeRepository.SearchAttributes(searchTerm);
//        }
//    }
//} 


using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MansehraPaintHouse.Infrastructure.Services
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly IProductAttributeRepository _productattributeRepository;

        public ProductAttributeService(IProductAttributeRepository productattributeRepository)
        {
            _productattributeRepository = productattributeRepository;
        }

        public async Task<ProductAttribute> GetAttributeByIdAsync(int id)
        {
            return await _productattributeRepository.GetByIdAsync(id);
        }

        public async Task<IQueryable<ProductAttribute>> GetAllAttributesQueryableAsync()
        {
            return _productattributeRepository.GetQueryable().Include(a => a.AttributeValues);
        }

        public async Task<IEnumerable<ProductAttribute>> GetActiveAttributesAsync()
        {
            return await _productattributeRepository.GetActiveAttributesAsync();
        }

        public async Task<ProductAttribute> CreateAttributeAsync(ProductAttribute attribute)
        {
            await _productattributeRepository.AddAsync(attribute);
            return attribute;
        }

        public async Task UpdateAttributeAsync(ProductAttribute attribute)
        {
            _productattributeRepository.Update(attribute);
        }

        public async Task DeleteAttributeAsync(int id)
        {
            var attribute = await GetAttributeByIdAsync(id);
            if (attribute != null)
            {
                _productattributeRepository.Remove(attribute);
            }
        }

        public async Task ToggleAttributeStatusAsync(int id)
        {
            await _productattributeRepository.ToggleAttributeStatusAsync(id);
        }

        public async Task<ProductAttribute> GetAttributeWithValuesAsync(int id)
        {
            return await _productattributeRepository.GetAttributeWithValuesAsync(id);
        }

        public async Task<IEnumerable<ProductAttribute>> GetAllAttributesAsync()
        {
            return await _productattributeRepository.GetAllAsync();
        }

        public async Task<IQueryable<ProductAttribute>> SearchAttributesAsync(string searchTerm)
        {
            return _productattributeRepository.SearchAttributes(searchTerm);
        }
    }
}