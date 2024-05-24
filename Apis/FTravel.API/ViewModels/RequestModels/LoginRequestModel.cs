using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        [Display(Name = "Email address")]
        public string Email { get; set; } = "";

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";
    }
}
