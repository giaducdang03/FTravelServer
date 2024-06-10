using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/stations")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationController(IStationService stationService)
        {
            _stationService = stationService;
        }

        //[HttpGet("stationList")]
        //public async Task<IActionResult> GetAllUser()
        //{
        //    try
        //    {
        //        var user = await _stationService.GetAllStationService();
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


        [HttpGet]
        public async Task<IActionResult> GetAllStation([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _stationService.GetAllStationService(paginationParameter);
                if (result == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Station is empty"
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
        public async Task<IActionResult> getStationDetailById(int id)
        {
            try
            {
                var data = await _stationService.GetStationServiceDetailById(id);
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
        [HttpPost("create-route")]
        public async Task<IActionResult> CreateRoute(RouteModel route)
        {
            try
            {
                var data = await _stationService.CreateRoute(route);
                if (route == null)
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

        [HttpPost("create-station")]
        public async Task<IActionResult> CreateStationController(StationModel station)
        {
            try
            {
                var data = await _stationService.CreateStationService(station);
                if (station == null)
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
    }
}
