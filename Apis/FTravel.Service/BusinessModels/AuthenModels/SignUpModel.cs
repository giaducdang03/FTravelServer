using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.AuthenModels
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        [Display(Name = "Email address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Full name is required!")]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = "";


        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Password must be 4-20 Character")]
        [PasswordPropertyText]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Confirm Password is required!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and password confirm does not match!")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Password must be 4-20 Character")]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; } = "";

        public RoleEnums Role { get; set; }
    }
}
