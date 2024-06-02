using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class ConfirmOtpModel
    {
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        [Display(Name = "Email")]

        public string Email { get; set; } = "";

        [StringLength(6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numeric values are allowed.")]
        public string OtpCode { get; set; } = "";
    }
}
