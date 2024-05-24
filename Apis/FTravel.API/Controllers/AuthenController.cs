using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FTravel.API.Controllers
{
    [Route("api/authen")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(SignUpModel model)
        {
            try
            {
                var result = await _userService.RegisterAsync(model);
                var resp = new ResponseModel()
                {
                    HttpCode = 200,
                    Message = "Create user succcess."
                };
                return Ok(resp);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = 400,
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
                var result = await _userService.LoginByEmailAndPassword(model.Email, model.Password);
                if (result.HttpCode == 200)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = 400,
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
                if (result.HttpCode == 200)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = 400,
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
