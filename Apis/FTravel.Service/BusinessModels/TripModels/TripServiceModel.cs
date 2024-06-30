using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.TripModels
{
    public class TripServiceModel
    {
        [Required(ErrorMessage = "ServiceId is required")]
        public int ServiceId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number")]
        public int Price { get; set; }
    }
}
