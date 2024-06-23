using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Service.BusinessModels.AuthenModels;
using FTravel.Service.Enums;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FTravel.API.Controllers
{
    [Route("api/authen")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IClaimsService _claimsService;

        public AuthenController(IUserService userService, IClaimsService claimsService)
        {
            _userService = userService;
            _claimsService = claimsService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(SignUpModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.RegisterAsync(model);
                    var resp = new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Mã OTP đã được gửi về email của bạn."
                    };
                    return Ok(resp);
                }
                return ValidationProblem(ModelState);

            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("check-user")]
        public async Task<IActionResult> CheckExistUser(CheckExistUser checkExistUser)
        {
            try
            {
                var existUser = await _userService.GetUserByEmailAsync(checkExistUser.Email);
                if (existUser != null)
                {
                    var resp = new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Tài khoản đã có trên hệ thống."
                    };
                    return Ok(resp);
                }
                else
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy tài khoản."
                    });
                }
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginWithEmail(LoginRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.LoginByEmailAndPassword(model.Email, model.Password);
                    if (result.HttpCode == StatusCodes.Status200OK)
                    {
                        return Ok(result);
                    }
                    return Unauthorized(result);
                }
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("login-with-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            try
            {
                var result = await _userService.LoginWithGoogle(credential);
                if (result.HttpCode == StatusCodes.Status200OK)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("confirmation")]
        public async Task<IActionResult> ConfirmEmail(ConfirmOtpModel confirmOtpModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.ConfirmEmail(confirmOtpModel);
                    if (result.HttpCode == StatusCodes.Status200OK)
                    {
                        return Ok(result);
                    }
                    return Unauthorized(result);
                }
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefresToken([FromBody] string jwtToken)
        {
            try
            {
                var result = await _userService.RefreshToken(jwtToken);
                if (result.HttpCode == StatusCodes.Status200OK)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> RequestResetPassword(ChangePasswordModel changePasswordModel)
        {
            try
            {
                var email = _claimsService.GetCurrentUserEmail;
                var result = await _userService.ChangePasswordAsync(email, changePasswordModel);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Đổi mật khẩu thành công."
                    });
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Không thể đổi mật khẩu."
                });
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RequestResetPassword([FromBody] string email)
        {
            try
            {
                var result = await _userService.RequestResetPassword(email);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Mã OTP đặt lại mật khẩu đã được gửi qua email."
                    });
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Đặt lại mật khẩu thất bại."
                });
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("reset-password/confirm")]
        public async Task<IActionResult> RequestResetPassword(ConfirmOtpModel confirmOtpModel)
        {
            try
            {
                var result = await _userService.ConfirmResetPassword(confirmOtpModel);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Bạn có thể đặt lại mật khẩu ngay bây giờ."
                    });
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "OTP không hợp lệ. Hãy thử lại."
                });
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("reset-password/new-password")]
        public async Task<IActionResult> RequestResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                var result = await _userService.ExecuteResetPassword(resetPasswordModel);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Đặt lại mật khẩu thành công."
                    });
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Đặt lại mật khẩu thất bại."
                });
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<IActionResult> GetLoginUserInfo()
        {
            try
            {
                var email = _claimsService.GetCurrentUserEmail;
                var result = await _userService.GetLoginUserInformationAsync(email);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Không thể lấy thông tin tài khoản."
                });
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpGet("test-admin")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> TestAdmin()
        {
            return Ok("Admin ne");
        }
    }
}
