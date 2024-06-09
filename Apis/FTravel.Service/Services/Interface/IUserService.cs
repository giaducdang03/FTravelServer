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

        public Task<AuthenModel> ConfirmEmail(ConfirmOtpModel confirmOtpModel);

        public Task<bool> RequestResetPassword(string email);

        public Task<bool> ConfirmResetPassword(ConfirmOtpModel confirmOtpModel);

        public Task<bool> ExecuteResetPassword(ResetPasswordModel resetPasswordModel);

        public Task<bool> ChangePasswordAsync(string email, ChangePasswordModel changePasswordModel);

        public Task<AuthenModel> LoginWithGoogle(string credental);

        public Task<UserModel> GetLoginUserInformationAsync(string email);
    }
}
