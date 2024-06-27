using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class UpdateFcmTokenModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string FcmToken { get; set; } = "";
    }
}
