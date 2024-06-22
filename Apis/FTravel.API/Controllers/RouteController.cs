using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services;
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
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> GetListRoute([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _routeService.GetListRouteAsync(paginationParameter);
                if(result == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không có tuyến đường nào"
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
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> GetRouteDetails(int id)
        {
            try
            {
                var routeDetail = await _routeService.GetRouteDetailByRouteIdAsync(id);
                if(routeDetail == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Tuyến đường không tồn tại"
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

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> UpdateRoute([FromBody] UpdateRouteModel routeUpdate, [FromRoute] int id)
        {
            try
            {
                var updateResult = await _routeService.UpdateRouteAsync(routeUpdate, id);
                if(updateResult > 0)
                {
                    return Ok(new ResponseModel() 
                    { 
                        HttpCode = StatusCodes.Status200OK, 
                        Message = "Đã cập nhật tuyến đường thành công" 
                    });

                }
                else
                {
                    return NotFound(new ResponseModel() 
                    { 
                        HttpCode = StatusCodes.Status404NotFound, 
                        Message = "Tuyến đường không tồn tại" 
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
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
                        Message = "Xóa tuyến đường thành công"
                    });
                } else
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Tuyến đường không tồn tại"
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
        [HttpPost]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> CreateRoute(CreateRouteModel route)
        {
            try
            {
                var data = await _routeService.CreateRoute(route);
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
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }

        }

    }
}
