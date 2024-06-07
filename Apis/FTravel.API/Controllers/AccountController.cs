using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FTravel.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("accountList")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var user = await _accountService.GetAllUserAscyn();
                return Ok(user);
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

        [HttpGet("accountById")]
        public async Task<IActionResult> GetAccountInfoByEmail(string email)
        {
            try
            {
                var data = await _accountService.GetAccountInfoByEmail(email);
                if(email == null)
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
                        Message = "Create account successfully. Please check email to login to FTravel."
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
    }
}
