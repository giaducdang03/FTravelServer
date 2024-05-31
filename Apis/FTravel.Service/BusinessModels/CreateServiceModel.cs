using System.ComponentModel.DataAnnotations;

namespace FTravel.Service.BusinessModels
{
    public class CreateServiceModel
    {
        [Required(ErrorMessage = "RouteId is required")]
        public int RouteId { get; set; }

        [Required(ErrorMessage = "StationId is required")]
        public int StationId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal DefaultPrice { get; set; }

        public string? ImgUrl { get; set; }

        [StringLength(100, ErrorMessage = "ShortDescription cannot exceed 100 characters")]
        public string? ShortDescription { get; set; }

        [StringLength(500, ErrorMessage = "FullDescription cannot exceed 500 characters")]
        public string? FullDescription { get; set; }
    }
}
