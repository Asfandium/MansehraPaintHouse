using System.ComponentModel.DataAnnotations;

namespace MansehraPaintHouse.Core.Entities
{
    public class ProductVariationAttribute
    {
        [Key]
        public int VariationAttributeID { get; set; }

        [Required(ErrorMessage = "Variation is required")]
        public int VariationID { get; set; }

        [Required(ErrorMessage = "Attribute is required")]
        public int AttributeID { get; set; }

        [Required(ErrorMessage = "Attribute Value is required")]
        public int AttributeValueID { get; set; }

        // Navigation properties
        public ProductVariation ProductVariation { get; set; }
        public ProductAttribute Attribute { get; set; }
        public AttributeValue AttributeValue { get; set; }
    }
} 