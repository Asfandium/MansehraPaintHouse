using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using MansehraPaintHouse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MansehraPaintHouse.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _dbSet.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductID == id);
        }

        public async Task<bool> IsProductActiveAsync(int id)
        {
            var product = await GetByIdAsync(id);
            return product?.IsActive ?? false;
        }

        public async Task ToggleProductStatusAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                product.IsActive = !product.IsActive;
                Update(product);
            }
        }

        public IQueryable<Product> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _dbSet.AsQueryable();
            }

            searchTerm = searchTerm.ToLower();
            return _dbSet.Where(p =>
                p.Name.ToLower().Contains(searchTerm) ||
                p.Description.ToLower().Contains(searchTerm) ||
                p.SKU.ToLower().Contains(searchTerm) ||
                (searchTerm == "active" && p.IsActive) ||
                (searchTerm == "inactive" && !p.IsActive)
            );
        }
    }
}
