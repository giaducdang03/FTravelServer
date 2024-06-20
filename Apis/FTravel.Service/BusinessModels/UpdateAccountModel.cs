using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class UpdateAccountModel
    {
        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string Dob { get; set; }

        public int? Gender { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
