using AutoMapper;
using Azure.Core;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOtpService _otpService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IRoleRepository roleRepository,
            ICustomerRepository customerRepository,
            IOtpService otpService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _customerRepository = customerRepository;
            _otpService = otpService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthenModel> LoginByEmailAndPassword(string email, string password)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            { 
                try
                {
                    var existUser = await _userRepository.GetUserByEmailAsync(email);
                    if (existUser == null)
                    {
                        return new AuthenModel
                        {
                            HttpCode = 401,
                            Message = "Tài khoản không tồn tại."
                        };
                    }
                    var verifyUser = PasswordUtils.VerifyPassword(password, existUser.PasswordHash);
                    if (verifyUser)
                    {
                        // check status user
                        if (existUser.Status == UserStatus.BANNED.ToString() || existUser.IsDeleted == true)
                        {
                            return new AuthenModel
                            {
                                HttpCode = 401,
                                Message = "Tài khoản đã bị cấm."
                            };
                        }

                        if (existUser.ConfirmEmail == false)
                        {
                            // send otp email
                            await _otpService.CreateOtpAsync(existUser.Email, "confirm");

                            await transaction.CommitAsync();

                            return new AuthenModel
                            {
                                HttpCode = 401,
                                Message = "Bạn phải xác nhận email trước khi đăng nhập vào hệ thống. OTP đã gửi qua email."
                            };
                        }

                        var accessToken = await GenerateAccessToken(email, existUser);
                        var refreshToken = GenerateRefreshToken(email);

                        await transaction.CommitAsync();

                        return new AuthenModel
                        {
                            HttpCode = 200,
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                    return new AuthenModel
                    {
                        HttpCode = 401,
                        Message = "Sai mật khẩu."
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
                
        }

        public async Task<AuthenModel> RefreshToken(string jwtToken)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigningKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                SecurityToken validatedToken;
                var principal = handler.ValidateToken(jwtToken, validationParameters, out validatedToken);
                var email = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                if (email != null)
                {
                    var existUser = await _userRepository.GetUserByEmailAsync(email);
                    if (existUser != null)
                    {
                        var accessToken = await GenerateAccessToken(email, existUser);
                        var refreshToken = GenerateRefreshToken(email);
                        return new AuthenModel
                        {
                            HttpCode = 200,
                            Message = "Refresh token successfully.",
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                }
                return new AuthenModel
                {
                    HttpCode = 401,
                    Message = "Tài khoản không tồn tại."
                };
            }
            catch
            {
                throw new Exception("Token không hợp lệ");
            }

        }

        public async Task<bool> RegisterAsync(SignUpModel model)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
                    User newUser = new User()
                    {
                        Email = model.Email,
                        FullName = model.FullName,
                        UnsignFullName = StringUtils.ConvertToUnSign(model.FullName),
                        Status = UserStatus.ACTIVE.ToString()
                    };

                    var existUser = await _userRepository.GetUserByEmailAsync(model.Email);

                    if (existUser != null)
                    {
                        throw new Exception("Tài khoản đã tồn tại.");
                    }

                    // hash password
                    newUser.PasswordHash = PasswordUtils.HashPassword(model.Password);

                    var role = await _roleRepository.GetRoleByName(model.Role.ToString());
                    if (role == null)
                    {
                        Role newRole = new Role
                        {
                            Name = model.Role.ToString()
                        };
                        await _roleRepository.AddAsync(newRole);
                        role = newRole;
                    }

                    newUser.RoleId = role.Id;

                    await _userRepository.AddAsync(newUser);

                    if (role.Name == RoleEnums.CUSTOMER.ToString())
                    {
                        if (await CheckExistCustomer(newUser.Email) == false)
                        {
                            Customer newCustomer = _mapper.Map<Customer>(newUser);
                            newCustomer.Id = 0;

                            // add wallet
                            Wallet customerWallet = new Wallet
                            {
                                AccountBalance = 0,
                                Status = WalletStatus.ACTIVE.ToString(),
                            };
                            newCustomer.Wallet = customerWallet;

                            await _customerRepository.AddAsync(newCustomer);

                            // send otp email
                            await _otpService.CreateOtpAsync(newCustomer.Email, "confirm");

                        }
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }

        public async Task<AuthenModel> ConfirmEmail(ConfirmOtpModel confirmOtpModel)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            { 
                try
                {
                    bool checkOtp = await _otpService.ValidateOtpAsync(confirmOtpModel.Email, confirmOtpModel.OtpCode);

                    if (checkOtp)
                    {
                        // return accesstoken

                        var existUser = await _userRepository.GetUserByEmailAsync(confirmOtpModel.Email);

                        if (existUser == null)
                        {
                            return new AuthenModel
                            {
                                HttpCode = 401,
                                Message = "Tài khoản không tồn tại."
                            };
                        }

                        // update confirm email for user
                        existUser.ConfirmEmail = true;
                        await _userRepository.UpdateAsync(existUser);

                        var accessToken = await GenerateAccessToken(confirmOtpModel.Email, existUser);
                        var refreshToken = GenerateRefreshToken(confirmOtpModel.Email);

                        await transaction.CommitAsync();

                        return new AuthenModel
                        {
                            HttpCode = 200,
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }

                    return new AuthenModel
                    {
                        HttpCode = 401,
                        Message = "OTP không hợp lệ."
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> RequestResetPassword(string email)
        {
            var existUser = await _userRepository.GetUserByEmailAsync(email);

            if (existUser != null)
            {
                if (existUser.ConfirmEmail == true)
                {
                    await _otpService.CreateOtpAsync(email, "reset");
                    return true;
                }
            }
            else
            {
                throw new Exception("Tài khoản không tồn tại.");
            }
            return false;
        }

        public async Task<bool> ConfirmResetPassword(ConfirmOtpModel confirmOtpModel)
        {
            return await _otpService.ValidateOtpAsync(confirmOtpModel.Email, confirmOtpModel.OtpCode);
        }

        private async Task<bool> CheckExistCustomer(string email)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            return true ? customer != null : false;
        }

        public async Task<bool> ExecuteResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var user = await _userRepository.GetUserByEmailAsync(resetPasswordModel.Email);
            if (user != null) 
            {
                user.PasswordHash = PasswordUtils.HashPassword(resetPasswordModel.Password);
                await _userRepository.UpdateAsync(user);
                return true;
            }
            else
            {
                throw new Exception("Tài khoản không tồn tại.");
            }
        }

        public async Task<bool> ChangePasswordAsync(string email, ChangePasswordModel changePasswordModel)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null) 
            { 
                bool checkPassword = PasswordUtils.VerifyPassword(changePasswordModel.OldPassword, user.PasswordHash);
                if (checkPassword) 
                { 
                    user.PasswordHash = PasswordUtils.HashPassword(changePasswordModel.NewPassword);
                    await _userRepository.UpdateAsync(user);
                    return true;
                }
                else 
                {
                    throw new Exception("Mật khẩu cũ không đúng.");
                }
            }
            else
            {
                throw new Exception("Tài khoản không tồn tại.");
            }
        }

        public async Task<AuthenModel> LoginWithGoogle(string credental)
        {
            string cliendId = _configuration["GoogleCredential:ClientId"];

            if (string.IsNullOrEmpty(cliendId))
            {
                throw new Exception("ClientId is null");
            }

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { cliendId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credental, settings);
            if (payload == null)
            {
                throw new Exception("Credental không hợp lệ.");
            }

            var existUser = await _userRepository.GetUserByEmailAsync(payload.Email);

            if (existUser != null)
            {
                var roleUser = await _roleRepository.GetByIdAsync(existUser.RoleId.Value);

                if (roleUser.Name != RoleEnums.CUSTOMER.ToString())
                {
                    throw new Exception("Tài khoản của bạn không được phép đăng nhập với Google.");
                }

                if (existUser.Status == UserStatus.BANNED.ToString())
                {
                    throw new Exception("Tài khoản đã bị cấm.");
                }
                else
                {
                    // create accesstoken
                    var accessToken = await GenerateAccessToken(existUser.Email, existUser);
                    var refreshToken = GenerateRefreshToken(existUser.Email);

                    return new AuthenModel()
                    {
                        HttpCode = 200,
                        Message = "Login with Google sucessfully",
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    };
                }
            }
            else
            {
                // create new customer account

                using (var transaction = await _userRepository.BeginTransactionAsync())
                {
                    try
                    {
                        User newUser = new User()
                        {
                            Email = payload.Email,
                            FullName = payload.Name,
                            ConfirmEmail = true,
                            UnsignFullName = StringUtils.ConvertToUnSign(payload.Name),
                            AvatarUrl = payload.Picture,
                            Status = UserStatus.ACTIVE.ToString(),
                            GoogleId = payload.JwtId
                        };

                        var role = await _roleRepository.GetRoleByName(RoleEnums.CUSTOMER.ToString());
                        if (role == null)
                        {
                            Role newRole = new Role
                            {
                                Name = RoleEnums.CUSTOMER.ToString()
                            };
                            await _roleRepository.AddAsync(newRole);
                            role = newRole;
                        }

                        newUser.RoleId = role.Id;

                        await _userRepository.AddAsync(newUser);

                        if (role.Name == RoleEnums.CUSTOMER.ToString())
                        {
                            if (await CheckExistCustomer(newUser.Email) == false)
                            {
                                Customer newCustomer = _mapper.Map<Customer>(newUser);
                                newCustomer.Id = 0;

                                // add wallet
                                Wallet customerWallet = new Wallet
                                {
                                    AccountBalance = 0,
                                    Status = WalletStatus.ACTIVE.ToString(),
                                };
                                newCustomer.Wallet = customerWallet;

                                await _customerRepository.AddAsync(newCustomer);

                            }
                        }
                        await transaction.CommitAsync();

                        // create accesstoken
                        var accessToken = await GenerateAccessToken(newUser.Email, newUser);
                        var refreshToken = GenerateRefreshToken(newUser.Email);

                        return new AuthenModel()
                        {
                            HttpCode = 200,
                            Message = "Login with Google sucessfully",
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        private async Task<string> GenerateAccessToken(string email, User user)
        {
            var role = await _roleRepository.GetByIdAsync(user.RoleId.Value);

            var authClaims = new List<Claim>();

            if (role != null)
            {
                authClaims.Add(new Claim(ClaimTypes.Email, email));
                authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                authClaims.Add(new Claim(ClaimTypes.Role, role.Name));
                //authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            }
            var accessToken = GenerateJWTToken.CreateToken(authClaims, _configuration, DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        private string GenerateRefreshToken(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
            };
            var refreshToken = GenerateJWTToken.CreateRefreshToken(claims, _configuration, DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(refreshToken).ToString();
        }

        public async Task<UserModel> GetLoginUserInformationAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                UserModel userModel = _mapper.Map<UserModel>(user);

                // get role
                var userRole = await _roleRepository.GetByIdAsync(user.RoleId.Value);
                if (userRole != null)
                {
                    userModel.Role = userRole.Name;
                    return userModel;
                }
            }
            return null;
        }

    }
}
