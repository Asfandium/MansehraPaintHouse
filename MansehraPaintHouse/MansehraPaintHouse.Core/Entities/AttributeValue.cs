using System.ComponentModel.DataAnnotations;

namespace MansehraPaintHouse.Core.Entities
{
    public class AttributeValue
    {
        [Key]
        public int AttributeValueID { get; set; }

        [Required(ErrorMessage = "Attribute is required")]
        public int ProductAttributeID { get; set; }

        [Required(ErrorMessage = "Value is required")]
        [StringLength(255, ErrorMessage = "Value cannot be longer than 255 characters")]
        public string Value { get; set; }

        [StringLength(255, ErrorMessage = "Code cannot be longer than 255 characters")]
        public string? Code { get; set; }

        [StringLength(7, ErrorMessage = "Color code must be 7 characters (e.g., #FFFFFF)")]
        public string? ColorCode { get; set; }

        public bool IsActive { get; set; } = true;
        // Navigation properties
        public ProductAttribute ProductAttribute { get; set; }
        public ICollection<ProductVariationAttribute> ProductVariationAttributes { get; set; }
    }
} 