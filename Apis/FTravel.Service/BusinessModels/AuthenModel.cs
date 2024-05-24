using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class AuthenModel
    {
        public int HttpCode { get; set; } = 200;

        public string Message { get; set; } = "";

        public string AccessToken { get; set; } = "";

        public string RefreshToken { get; set; } = "";
    }
}
