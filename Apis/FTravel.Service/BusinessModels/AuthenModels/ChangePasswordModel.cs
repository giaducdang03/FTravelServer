using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.AuthenModels
{
    public class ChangePasswordModel
    {

        [Required(ErrorMessage = "Old password is required!")]
        public string OldPassword { get; set; } = "";

        [Required(ErrorMessage = "New password is required!")]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Password must be 4-20 Character")]
        [PasswordPropertyText]
        public string NewPassword { get; set; } = "";
    }
}
