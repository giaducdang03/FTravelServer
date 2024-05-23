using AutoMapper;
using Azure.Core;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
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
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IRoleRepository roleRepository,
            ICustomerRepository customerRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _customerRepository = customerRepository;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<AuthenModel> LoginByEmailAndPassword(string email, string password)
        {
            var existUser = await _userRepository.GetUserByEmailAsync(email);
            if (existUser == null)
            {
                return new AuthenModel
                {
                    HttpCode = 401,
                    Message = "Account does not exist"
                };
            }
            var verifyUser = PasswordUtils.VerifyPassword(password, existUser.PasswordHash);
            if (verifyUser)
            {
                var accessToken = await GenerateAccessToken(email, existUser);
                var refreshToken = GenerateRefreshToken(email);

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
                Message = "Wrong password"
            };
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
                    Message = "User does not exist."
                };
            }
            catch
            {
                throw new Exception("Token is not valid.");
            }

        }

        public async Task<bool> RegisterAsync(SignUpModel model)
        {
            User newUser = new User()
            {
                Email = model.Email,
                FullName = model.FullName,
                UnsignFullName = StringUtils.ConvertToUnSign(model.FullName)
            };

            var existUser = await _userRepository.GetUserByEmailAsync(model.Email);

            if (existUser != null)
            {
                throw new Exception("Account is already exist.");
            }

            // hash password
            newUser.PasswordHash = PasswordUtils.HashPassword(model.Password);

            if (await _roleRepository.GetRoleByName(model.Role.ToString()) == null)
            {
                Role newRole = new Role
                {
                    Name = model.Role.ToString()
                };
                await _roleRepository.AddAsync(newRole);
            }

            var role = await _roleRepository.GetRoleByName(model.Role.ToString());

            if (role != null && role.Name == RoleEnums.CUSTOMER.ToString())
            {
                newUser.RoleId = role.Id;
                if (CheckExistCustomer(newUser.Email).Result == false)
                {
                    try
                    {
                        await _userRepository.AddAsync(newUser);
                        Customer newCustomer = _mapper.Map<Customer>(newUser);
                        newCustomer.Id = 0;
                        await _customerRepository.AddAsync(newCustomer);
                    }
                    catch
                    {
                        await _userRepository.PermanentDeletedAsync(newUser);
                    }
                }
                return true;
            }
            else if (role != null)
            {
                newUser.RoleId = role.Id;
                await _userRepository.AddAsync(newUser);
                return true;
            }
            return false;
        }

        private async Task<bool> CheckExistCustomer(string email)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            return true ? customer != null : false;
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
    }
}
