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

        public string? PasswordHash { get; set; }

        public string? UnsignFullName { get; set; }

        public string FullName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public int? Gender { get; set; }

        public string? Status { get; set; }

        public string? AvatarUrl { get; set; }

        public int? RoleId { get; set; }

    }
}
