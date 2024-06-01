using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class SettingRequestModel
    {
        [Required]
        public string Key { get; set; } = "";


        [Required]
        public string Value { get; set; } = "";

        public string Decription { get; set; } = "";
    }
}
