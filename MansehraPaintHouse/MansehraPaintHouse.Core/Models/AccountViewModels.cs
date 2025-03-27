//using System.ComponentModel.DataAnnotations;

//namespace MansehraPaintHouse.Core.Models
//{
//    public class LoginViewModel
//    {
//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }

//        [Display(Name = "Remember me?")]
//        public bool RememberMe { get; set; }
//    }

//    public class RegisterViewModel
//    {
//        [Required]
//        [StringLength(50)]
//        [Display(Name = "First Name")]
//        public string FirstName { get; set; }

//        [Required]
//        [StringLength(50)]
//        [Display(Name = "Last Name")]
//        public string LastName { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
//        [DataType(DataType.Password)]
//        [Display(Name = "Password")]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Confirm password")]
//        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//        public string ConfirmPassword { get; set; }
//    }

//    public class ForgotPasswordViewModel
//    {
//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }
//    }

//    public class ResetPasswordViewModel
//    {
//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Confirm password")]
//        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//        public string ConfirmPassword { get; set; }

//        public string Code { get; set; }
//    }
//} 