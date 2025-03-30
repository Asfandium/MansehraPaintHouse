using System.ComponentModel.DataAnnotations;

namespace MansehraPaintHouse.Core.Entities
{
    public class ProductAttribute
    {
        [Key]
        public int ProductAttributeID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<AttributeValue> AttributeValues { get; set; }
    }
} 