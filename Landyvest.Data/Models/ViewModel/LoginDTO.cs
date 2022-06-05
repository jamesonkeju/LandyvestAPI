using System.ComponentModel.DataAnnotations;

namespace Landyvest.Data.Models.ViewModel
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "Username/Email is required")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordViewModelNew
    {
      
            //public LoginDTO loginDTO { get; set; }
            [Required(ErrorMessage = "Username/Email is required")]
            public string username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        
    }

    public class ChangePasswordViewModel
    {
        //public LoginDTO loginDTO { get; set; }
        [Required(ErrorMessage = "Username/Email is required")]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    public class PasswordChangeViewModel
    {

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Id { get; set; }
    }


    public class ResetChangePasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        public string token { get; set; }
        public string userid { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgetPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool IsMobile { get; set; } = false;

    }
}
