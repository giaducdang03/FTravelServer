using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class CheckExistUser
    {
        [Required]
        [EmailAddress(ErrorMessage = "Must be email format!")]
        public string Email { get; set; } = "";
    }
}
