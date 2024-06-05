using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;


        public TripController(ITripService tripService)
        {
            _tripService = tripService;

        }

        [HttpGet]
        [Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> GetAllTripStatusOpening([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _tripService.GetAllTripsWithStatusOpening(paginationParameter);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No trips was found"
                    });
                }


                else
                {
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
                }


                return Ok(result);
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

        [HttpGet]
        [Route("get-by-id")]
        [Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> GetTripDetailByIdStatusOpening([FromQuery] int id)
        {
            try
            {
                var result = await _tripService.GetTripDetailByIdWithStatusOpening(id);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No trip was found"
                    });
                }

                return Ok(result);
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
