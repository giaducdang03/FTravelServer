using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class UpdateTicketTypeModel
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "RouteId không được để trống")]
        public int? RouteId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Giá tiền phải là số dương")]
        public int? Price { get; set; }

        
    }
}
