using MansehraPaintHouse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface IProductService
    {
        /// <summary>
        /// Retrieves a product by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the product or null if not found.</returns>
        Task<Product> GetProductByIdAsync(int id);

        /// <summary>
        /// Retrieves all products as a queryable collection, including their categories, asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing an IQueryable of products with categories.</returns>
        Task<IQueryable<Product>> GetAllProductsQueryableAsync();

        /// <summary>
        /// Retrieves all active products asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a collection of active products.</returns>
        Task<IEnumerable<Product>> GetActiveProductsAsync();

        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a collection of all products.</returns>
        Task<IEnumerable<Product>> GetAllProductsAsync();

        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <returns>A task that represents the asynchronous operation, containing the created product.</returns>
        Task<Product> CreateProductAsync(Product product);

        /// <summary>
        /// Updates an existing product asynchronously.
        /// </summary>
        /// <param name="product">The product to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateProductAsync(Product product);

        /// <summary>
        /// Deletes a product by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteProductAsync(int id);

        /// <summary>
        /// Toggles the active status of a product asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to toggle.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ToggleProductStatusAsync(int id);

        /// <summary>
        /// Retrieves a product by its ID, including its associated category, asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the product with its category, or null if not found.</returns>
        Task<Product> GetProductWithCategoryAsync(int id);

        /// <summary>
        /// Searches products based on a search term asynchronously.
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <returns>A task that represents the asynchronous operation, containing an IQueryable of products matching the search criteria.</returns>
        Task<IQueryable<Product>> SearchProductsAsync(string searchTerm);
    }
}
