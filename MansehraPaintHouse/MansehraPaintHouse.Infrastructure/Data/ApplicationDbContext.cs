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
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<ProductVariationAttribute> ProductVariationAttributes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category relationships
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Product relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductVariation relationships
            modelBuilder.Entity<ProductVariation>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.ProductVariations)
                .HasForeignKey(pv => pv.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // AttributeValue relationships
            modelBuilder.Entity<AttributeValue>()
                .HasOne(av => av.ProductAttribute)
                .WithMany(a => a.AttributeValues)
                .HasForeignKey(av => av.ProductAttributeID)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductVariationAttribute relationships
            modelBuilder.Entity<ProductVariationAttribute>()
                .HasOne(pva => pva.ProductVariation)
                .WithMany(pv => pv.ProductVariationAttributes)
                .HasForeignKey(pva => pva.VariationID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariationAttribute>()
                .HasOne(pva => pva.Attribute)
                .WithMany()
                .HasForeignKey(pva => pva.AttributeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductVariationAttribute>()
                .HasOne(pva => pva.AttributeValue)
                .WithMany(av => av.ProductVariationAttributes)
                .HasForeignKey(pva => pva.AttributeValueID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
