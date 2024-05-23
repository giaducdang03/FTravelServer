using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class UserModel
    {
        public string Email { get; set; } = null!;

        public bool? ConfirmEmail { get; set; } = false;

        public string? PasswordHash { get; set; }

        public string FullName { get; set; } = null!;

        public DateTime? Dob { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public int? Gender { get; set; }

        public string? AvatarUrl { get; set; } = null;

    }
}
