using MansehraPaintHouse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MansehraPaintHouse.Core.Interfaces.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        /// <summary>
        /// Retrieves all active products asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a collection of active products.</returns>
        Task<IEnumerable<Product>> GetActiveProductsAsync();

        /// <summary>
        /// Retrieves a product by its ID, including its associated category, asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the product with its category, or null if not found.</returns>
        Task<Product> GetProductWithCategoryAsync(int id);

        /// <summary>
        /// Checks if a product is active asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to check.</param>
        /// <returns>A task that represents the asynchronous operation, containing a boolean indicating if the product is active.</returns>
        Task<bool> IsProductActiveAsync(int id);

        /// <summary>
        /// Toggles the active status of a product asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to toggle.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ToggleProductStatusAsync(int id);

        /// <summary>
        /// Searches products based on a search term, matching against name, description, SKU, or status.
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <returns>An IQueryable of products matching the search criteria.</returns>
        IQueryable<Product> SearchProducts(string searchTerm);
    }
}
