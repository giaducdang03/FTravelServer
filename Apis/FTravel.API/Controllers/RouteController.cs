using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.RouteModels;
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
        [Authorize]
        public async Task<IActionResult> GetListRoute([FromQuery] PaginationParameter paginationParameter, [FromQuery(Name = "buscompany-id")] int? buscompanyId, [FromQuery] RouteFilter routeFilter)
        {
            try
            {
                var result = await _routeService.GetListRouteAsync(paginationParameter, buscompanyId, routeFilter);
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
        [Authorize]
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
                else if(updateResult == -1)
                {
                    return NotFound(new ResponseModel() 
                    { 
                        HttpCode = StatusCodes.Status404NotFound, 
                        Message = "Tuyến đường không tồn tại" 
                    });

                } else
                {
                    return BadRequest(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Điểm đầu và điểm cuối không được trùng nhau"
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

        [HttpPost("add-station")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> AddStationForRoute([FromBody] AddStationForRouteModel addStation)
        {
            try
            {
                var data = await _routeService.AddStationForRoute(addStation);
                if(data > 0)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Thêm trạm cho tuyến đường thành công"
                    });
                } else if(data == -2)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Đã có trạm tại vị trí này"
                    });
                } else if(data == -3)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Đã có trạm này trong tuyến đường"
                    });
                } else
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy trạm cho tuyến đường"
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                });
               
            }
        }

        [HttpPut("change-station-index")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> ChangeStationIndex([FromBody] IEnumerable<ChangeStationModel> changeStation)
        {
            try
            {
                var result = await _routeService.ChangeStationIndex(changeStation);
                if(result)
                {
                    return Ok(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Cập nhật thứ tự trạm thành công"
                    });
                } else
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Vui lòng chọn 1 tuyến đường và 2 trạm khác nhau để cập nhật"
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
