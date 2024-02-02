//Developed by Nayan

using System.ComponentModel.DataAnnotations;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirm new password do not match.")]
        public string ConfirmNewPassword { get; set; }

        
    }
}