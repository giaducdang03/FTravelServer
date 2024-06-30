using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.AccountModels
{
    public class AccountModel : BaseEntity
    {

        public string Email { get; set; } = "";

        public bool ConfirmEmail { get; set; } = false;

        public string FullName { get; set; } = "";

        public DateTime? Dob { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public int? Gender { get; set; }

        public string? Status { get; set; }

        public string? AvatarUrl { get; set; }

        public string Role { get; set; } = "";

    }
}
