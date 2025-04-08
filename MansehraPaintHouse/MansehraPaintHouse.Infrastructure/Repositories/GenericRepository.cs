using Microsoft.EntityFrameworkCore;
using MansehraPaintHouse.Core.Interfaces.IRepositories;
using System.Linq.Expressions;
using MansehraPaintHouse.Infrastructure.Data;

namespace MansehraPaintHouse.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error in AddAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw the exception to be handled by the service
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error in AddRangeAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw the exception to be handled by the service
            }
        }

        public void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error in Update: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw the exception to be handled by the service
            }
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
} 