using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.EntityModels
{
    public partial class Otp : BaseEntity
    {
        public string Email { get; set; } = "";

        public string OtpCode { get; set; } = "";

        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}
