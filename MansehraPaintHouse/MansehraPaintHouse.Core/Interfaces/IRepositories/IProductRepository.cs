using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<Product> GetProductWithCategoryAsync(int id);
        Task<bool> IsProductActiveAsync(int id);
        Task ToggleProductStatusAsync(int id);
        IQueryable<Product> SearchProducts(string searchTerm);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsByParentCategoryAsync(int parentCategoryId);
    }
} 