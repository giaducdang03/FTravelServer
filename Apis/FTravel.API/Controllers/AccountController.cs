using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Web.WebPages.Html;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FTravel.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllUserAccount([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _accountService.GetAllUserAccountService(paginationParameter);
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

        //[HttpGet("account-list")]
        //public async Task<IActionResult> GetAllUser()
        //{
        //    try
        //    {
        //        var user = await _accountService.GetAllUserAscyn();
        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = 400,
        //            Message = ex.Message
        //        });
        //    }

        //}

        [HttpPut("{id}/status")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> BanAccountControl(int id)
        {
            try
            {

                var result = await _accountService.BanAccount(id);
                if (id == null)
                {
                    return BadRequest();
                }
                return Ok(result);
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
        [HttpPut("{id}/update")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateService(int id, UpdateAccountModel accountModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isUpdated = await _accountService.UpdateAccount(id, accountModel);

                if (isUpdated)
                {
                    // Return a success response
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Account updated successfully"
                    });
                }
                else
                {
                    // Return a not found response if the service was not updated successfully
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Failed to update the Account"
                    });
                }
            }
            catch (Exception ex)
            {
                // Return a bad request response for any other exceptions
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }

}
