using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FTravel.API.Controllers
{
    [Route("api/firebases")]
    [ApiController]
    public class FirebasesController : ControllerBase
    {
        [HttpPost("push-noti")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PushNotiFirebase(FirebaseRequestModel firebaseRequestModel)
        {
            try
            {
                await FirebaseLibrary.SendMessageFireBase(firebaseRequestModel.Title, firebaseRequestModel.Body, firebaseRequestModel.Token);
                return Ok(new ResponseModel()
                {
                    HttpCode = 200,
                    Message = "Push noti successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }
    }
}
