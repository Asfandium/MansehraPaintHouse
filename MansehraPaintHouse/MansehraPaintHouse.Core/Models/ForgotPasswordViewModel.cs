using System.ComponentModel.DataAnnotations;

namespace MansehraPaintHouse.Core.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
} 