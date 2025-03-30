using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MansehraPaintHouse.Infrastructure.Services
{
    public class ProductVariationService : IProductVariationService
    {
        private readonly IProductVariationRepository _productVariationRepository;

        public ProductVariationService(IProductVariationRepository productVariationRepository)
        {
            _productVariationRepository = productVariationRepository;
        }

        public async Task<ProductVariation> GetProductVariationByIdAsync(int id)
        {
            return await _productVariationRepository.GetByIdAsync(id);
        }

        public async Task<IQueryable<ProductVariation>> GetAllProductVariationsQueryableAsync()
        {
            return _productVariationRepository.GetQueryable()
                .Include(pv => pv.Product)
                .Include(pv => pv.ProductVariationAttributes)
                    .ThenInclude(pva => pva.Attribute)
                .Include(pv => pv.ProductVariationAttributes)
                    .ThenInclude(pva => pva.AttributeValue);
        }

        public async Task<IEnumerable<ProductVariation>> GetActiveProductVariationsAsync()
        {
            return await _productVariationRepository.GetActiveProductVariationsAsync();
        }

        public async Task<ProductVariation> CreateProductVariationAsync(ProductVariation productVariation)
        {
            await _productVariationRepository.AddAsync(productVariation);
            return productVariation;
        }

        public async Task UpdateProductVariationAsync(ProductVariation productVariation)
        {
            _productVariationRepository.Update(productVariation);
        }

        public async Task DeleteProductVariationAsync(int id)
        {
            var productVariation = await GetProductVariationByIdAsync(id);
            if (productVariation != null)
            {
                _productVariationRepository.Remove(productVariation);
            }
        }

        public async Task ToggleProductVariationStatusAsync(int id)
        {
            await _productVariationRepository.ToggleProductVariationStatusAsync(id);
        }

        public async Task<ProductVariation> GetProductVariationWithProductAsync(int id)
        {
            return await _productVariationRepository.GetProductVariationWithProductAsync(id);
        }

        public async Task<ProductVariation> GetProductVariationWithAttributesAsync(int id)
        {
            return await _productVariationRepository.GetProductVariationWithAttributesAsync(id);
        }

        public async Task<IEnumerable<ProductVariation>> GetAllProductVariationsAsync()
        {
            return await _productVariationRepository.GetAllAsync();
        }

        public async Task<IQueryable<ProductVariation>> SearchProductVariationsAsync(string searchTerm)
        {
            return _productVariationRepository.SearchProductVariations(searchTerm);
        }

        public async Task<IEnumerable<ProductVariation>> GetProductVariationsByProductAsync(int productId)
        {
            return await _productVariationRepository.GetProductVariationsByProductAsync(productId);
        }
    }
} 