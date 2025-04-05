using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MansehraPaintHouse.Core.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
        public string Name { get; set; }
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "Image path cannot be longer than 255 characters")]
        public string Image { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public Category Category { get; set; }
    }
}
