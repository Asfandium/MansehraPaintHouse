using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MansehraPaintHouse.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<IQueryable<Product>> GetAllProductsQueryableAsync()
        {
            return _productRepository.GetQueryable().Include(p => p.Category);
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _productRepository.GetActiveProductsAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            _productRepository.Update(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
            {
                _productRepository.Remove(product);
            }
        }

        public async Task ToggleProductStatusAsync(int id)
        {
            await _productRepository.ToggleProductStatusAsync(id);
        }

        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            return await _productRepository.GetProductWithCategoryAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<IQueryable<Product>> SearchProductsAsync(string searchTerm)
        {
            return _productRepository.SearchProducts(searchTerm);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetProductsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Product>> GetProductsByParentCategoryAsync(int parentCategoryId)
        {
            return await _productRepository.GetProductsByParentCategoryAsync(parentCategoryId);
        }
    }
} 