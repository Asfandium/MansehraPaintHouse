using Microsoft.EntityFrameworkCore;
using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}
