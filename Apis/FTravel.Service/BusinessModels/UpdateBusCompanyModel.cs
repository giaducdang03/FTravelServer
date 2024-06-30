using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class UpdateBusCompanyModel
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; } = null!;

        public string? ImgUrl { get; set; }

        [StringLength(100, ErrorMessage = "Mô tả không vượt quá 100 từ")]
        public string? ShortDescription { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả chi tiết không vượt quá 500 từ")]
        public string? FullDescription { get; set; }

        [Required(ErrorMessage = "ManagerEmail không được để trống")]
        public string ManagerEmail { get; set; } = null!;
    }
}
