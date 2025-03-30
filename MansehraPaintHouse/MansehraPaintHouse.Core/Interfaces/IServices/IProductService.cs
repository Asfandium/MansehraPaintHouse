using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IQueryable<Product>> GetAllProductsQueryableAsync();
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task ToggleProductStatusAsync(int id);
        Task<Product> GetProductWithCategoryAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IQueryable<Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsByParentCategoryAsync(int parentCategoryId);
    }
} 