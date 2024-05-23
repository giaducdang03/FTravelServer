using FTravel.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IUserService
    {
        public Task<AuthenModel> LoginByEmailAndPassword(string email, string password);

        public Task<bool> RegisterAsync(SignUpModel model);

        public Task<AuthenModel> RefreshToken(string jwtToken);
    }
}
