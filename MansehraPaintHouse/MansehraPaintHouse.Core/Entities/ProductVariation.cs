using System.ComponentModel.DataAnnotations;

namespace MansehraPaintHouse.Core.Entities
{
    public class ProductVariation
    {
        [Key]
        public int VariationID { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "Image path cannot be longer than 255 characters")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [StringLength(50, ErrorMessage = "SKU cannot be longer than 50 characters")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity must be greater than or equal to 0")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Minimum Stock Level is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum Stock Level must be greater than or equal to 0")]
        public int MinimumStockLevel { get; set; }

        [StringLength(20, ErrorMessage = "Color Code cannot be longer than 20 characters")]
        public string? ColorCode { get; set; }

        [StringLength(100, ErrorMessage = "Color Name cannot be longer than 100 characters")]
        public string? ColorName { get; set; }

        [StringLength(50, ErrorMessage = "Size cannot be longer than 50 characters")]
        public string? Size { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Product Product { get; set; }
        public ICollection<ProductVariationAttribute> ProductVariationAttributes { get; set; }
    }
} 