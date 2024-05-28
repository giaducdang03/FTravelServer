using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }


        [HttpGet("routes")]
        [Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> getListRoute([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _routeService.GetListRouteAsync(paginationParameter);
                if(result == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Route is empty"
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

        [HttpGet("route-details/{id}")]
        [Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> getRouteDetails(int id)
        {
            try
            {
                var routeDetail = await _routeService.GetRouteDetailByRouteIdAsync(id);
                if(routeDetail == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Route does not exist"
                    });
                }
                return Ok(routeDetail);
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
