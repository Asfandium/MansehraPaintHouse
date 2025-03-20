using System.ComponentModel.DataAnnotations;

namespace MansehraPaintHouse.Core.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        
        public int? ParentCategoryID { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Image1 path cannot be longer than 255 characters")]
        public string? Image1 { get; set; }
        
        [StringLength(255, ErrorMessage = "Image2 path cannot be longer than 255 characters")]
        public string? Image2 { get; set; }
        
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;

        // Navigation property
        public Category? ParentCategory { get; set; }
    }
} 