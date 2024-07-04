using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.TripModels
{
    public class CreateTripServiceModel
    {
        [Required(ErrorMessage = "Thiếu id của dịch vụ")]
        public int ServiceId { get; set; }

        [Range(0, 999, ErrorMessage = "Giá dịch vụ không được quá 999 token(999000 vnđ")]
        public int Price { get; set; }
    }
}
