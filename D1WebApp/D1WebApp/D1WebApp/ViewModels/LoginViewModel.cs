//Developed by Sunil

using System.ComponentModel.DataAnnotations;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        //[Required]
        //[StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRememberPassword { get; set; }
        public string Param { get; set; }
        public bool? LoginType { get; set; }
    }


    public class VendorLoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public long UserID { get; set; }
    }

    public class CheckUserViewModel
    {
        public string EmailID { get; set; }
        public decimal MobileNo { get; set; }
    }
}