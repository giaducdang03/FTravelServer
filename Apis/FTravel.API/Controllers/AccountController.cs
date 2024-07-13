using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.AccountModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IClaimsService _claimsService;

        public AccountController(IAccountService accountService, IClaimsService claimsService)
        {
            _accountService = accountService;
            _claimsService = claimsService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllUserAccount([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _accountService.GetAllUsersAsync(paginationParameter);
                if (result == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Account is empty"
                    });
                }

                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAccountInfoById(int id)
        {
            try
            {
                var data = await _accountService.GetAccountInfoById(id);
                if (id == null)
                {
                    return BadRequest();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel
                {
                    HttpCode = 400,
                    Message = ex.Message
                });
            }

        }

        [HttpGet("by-email/{email}")]
        [Authorize]
        public async Task<IActionResult> GetAccountInfoByEmail(string email)
        {
            try
            {
                var data = await _accountService.GetAccountInfoByEmail(email);
                if (email == null)
                {
                    return BadRequest();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel
                {
                    HttpCode = 400,
                    Message = ex.Message
                });
            }

        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateAccountInternal(CreateAccountModel account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.CreateAccountAsync(account);
                    var resp = new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Tạo tài khoản thành công. Vui lòng kiểm tra email để đăng nhập vào FTravel."
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteAccountById(int id)
        {
            try
            {
                var currentEmail = _claimsService.GetCurrentUserEmail;
                var result = await _accountService.DeleteAccountAsync(id, currentEmail);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Xóa người dùng thành công."
                    });
                }
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Có lỗi trong quá trình xóa người dùng."
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

        [HttpPut("update-fcm-token")]
        [Authorize]
        public async Task<IActionResult> UpdateFcmToken(UpdateFcmTokenModel updateFcmTokenModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var email = _claimsService.GetCurrentUserEmail;
                    if (email == updateFcmTokenModel.Email)
                    {
                        var result = await _accountService.UpdateFcmTokenAsync(email, updateFcmTokenModel.FcmToken);
                        if (result)
                        {
                            return Ok(new ResponseModel()
                            {
                                HttpCode = StatusCodes.Status200OK,
                                Message = "Update FCM token successfully."
                            });
                        }
                        return Ok(new ResponseModel()
                        {
                            HttpCode = StatusCodes.Status400BadRequest,
                            Message = "Update FCM token error."
                        });
                    }
                    return Ok(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "User does not exist."
                    });
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
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateService(UpdateAccountModel accountModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isUpdated = await _accountService.UpdateAccount(accountModel);

                    if (isUpdated)
                    {
                        return Ok(new ResponseModel
                        {
                            HttpCode = StatusCodes.Status200OK,
                            Message = "Cập nhật thông tin tài khoản thành công."
                        });
                    }
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Có lỗi trong quá trình cập nhật thông tin tài khoản."
                    });

                }
                return ValidationProblem(ModelState);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }
}
