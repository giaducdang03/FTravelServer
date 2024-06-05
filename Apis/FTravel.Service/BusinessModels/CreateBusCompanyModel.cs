using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class CreateBusCompanyModel
    {
        [Required(ErrorMessage = "The company name is required.")]
        [StringLength(100, ErrorMessage = "The company name cannot be longer than 100 characters.")]
        public string Name { get; set; } = null!;

        public string? ImgUrl { get; set; }

        [StringLength(200, ErrorMessage = "The short description cannot be longer than 200 characters.")]
        public string? ShortDescription { get; set; }

        public string? FullDescription { get; set; }
    }
}
