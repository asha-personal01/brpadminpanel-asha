//Developed by Sunil

using System.ComponentModel.DataAnnotations;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

    }


}