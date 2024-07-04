using System.ComponentModel.DataAnnotations;

namespace FTravel.Service.BusinessModels.ServiceModels
{
    public class CreateServiceModel
    {
        [Required(ErrorMessage = "Thiếu id của tuyến đường")]
        public int RouteId { get; set; }

        [Required(ErrorMessage = "Thiếu id của trạm")]
        public int StationId { get; set; }

        [Required(ErrorMessage = "Thiếu tên của dịch vụ")]
        public string Name { get; set; }

        [Range(0, 999, ErrorMessage = "Giá dịch vụ không được quá 999 token(999000 vnđ)")]
        public decimal DefaultPrice { get; set; }

        public string? ImgUrl { get; set; }

        [StringLength(100, ErrorMessage = "mô tả ngắn không được quá 100 ký tự")]
        public string? ShortDescription { get; set; }

        [StringLength(500, ErrorMessage = "mô tả dài không được quá 500 ký tự")]
        public string? FullDescription { get; set; }
    }
}
