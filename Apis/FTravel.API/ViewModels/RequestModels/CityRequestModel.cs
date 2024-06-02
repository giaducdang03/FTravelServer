using System.ComponentModel.DataAnnotations;

namespace FTravel.API.ViewModels.RequestModels
{
    public class CityRequestModel
    {
        [MaxLength(100)]
        public string? UnsignName { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
