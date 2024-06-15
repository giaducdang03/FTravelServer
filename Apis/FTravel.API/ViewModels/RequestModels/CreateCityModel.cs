using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class CreateCityModel
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public int Code { get; set; }
    }
}
