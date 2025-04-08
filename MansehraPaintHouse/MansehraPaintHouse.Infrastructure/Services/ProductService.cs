using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using MansehraPaintHouse.Core.Interfaces.IServices;
using MansehraPaintHouse.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                await _productRepository.AddAsync(product);
                return product;
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error in CreateProductAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw the exception to be handled by the controller
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                _productRepository.Update(product);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error in UpdateProductAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw the exception to be handled by the controller
            }
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

        public async Task<IQueryable<Product>> SearchProductsAsync(string searchTerm)
        {
            return _productRepository.SearchProducts(searchTerm);
        }
    }
}
