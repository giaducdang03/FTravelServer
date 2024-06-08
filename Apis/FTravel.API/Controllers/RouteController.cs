using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Route = FTravel.Repository.EntityModels.Route;

namespace FTravel.API.Controllers
{
    [Route("api/routes")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }


        [HttpGet]
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

        [HttpGet("{id}")]
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

        [HttpPut]
        //[Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> UpdateRoute(RouteRequestModel routeRequest)
        {
            try
            {
                var routeUpdate = new Route()
                {
                    Id = routeRequest.Id,
                    BusCompanyId = routeRequest.BusCompanyId,
                    StartPoint = routeRequest.StartPoint,
                    EndPoint = routeRequest.EndPoint,
                    Name = routeRequest.Name,
                    Status = routeRequest.Status,
                    UpdateDate = DateTime.Now,
                };
                var updateResult = await _routeService.UpdateRouteAsync(routeUpdate);
                if(updateResult > 0)
                {
                    return Ok(new ResponseModel() { HttpCode = StatusCodes.Status200OK, Message = "Update Success" });

                }
                else
                {
                    return NotFound(new ResponseModel() { HttpCode = StatusCodes.Status404NotFound, Message = "Not found Route to update" });

                }
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "BUSCOMPANY")]
        public async Task<IActionResult> RouteSoftDelete(int id)
        {
            try
            {
                var result = await _routeService.RouteSoftDeleteAsync(id);
                if(result > 0)
                {
                    return Ok(new ResponseModel()
                    {
                        HttpCode = 200,
                        Message = "Soft Delete Route Success"
                    });
                } else
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Route not found"
                    });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                });
            }
        }

        
    }
}
