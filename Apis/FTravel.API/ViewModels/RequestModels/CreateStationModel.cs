using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class CreateStationModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int BusCompanyId { get; set; }
    }
}
