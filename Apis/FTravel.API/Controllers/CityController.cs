using FTravel.API.ViewModels.RequestModels;
using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetListCity([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _cityService.GetListCityAsync(paginationParameter);
                if(result != null)
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
                    return Ok(result);  
                } else
                {
                   return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy thành phố"
                    });
                }
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };
                return BadRequest(responseModel);
            }
        }

        //[HttpPut("{id}")]
        //[Authorize(Roles = "ADMIN")]
        //public async Task<IActionResult> UpdateCity([FromBody] CityModel cityModel, [FromRoute] int id)
        //{
        //    try
        //    {
        //        var result = await _cityService.UpdateCityAsync(cityModel, id);
        //        if(result == null)
        //        {
        //            return NotFound(new ResponseModel()
        //            {
        //                HttpCode = StatusCodes.Status404NotFound,
        //                Message = "Không thể thay đổi thông tin thành phố"
        //            });
        //        }
        //        return Ok(new ResponseModel()
        //        {
        //            HttpCode = StatusCodes.Status200OK,

        //            Message = "Thay đổi thành công"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel()
        //        {
        //            HttpCode = StatusCodes.Status400BadRequest,
        //            Message = ex.Message
        //        });
        //    }
        //}

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityModel createCityModel)
		{
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _cityService.CreateCityAsync(createCityModel.Code, createCityModel.Name);
                    if (result <= 0)
                    {
                        return BadRequest(new ResponseModel()
                        {
                            HttpCode = StatusCodes.Status400BadRequest,
                            Message = "Không thể thêm thành phố này"
                        });
                    }
                    return Ok(new ResponseModel 
                    { 
                        HttpCode = StatusCodes.Status200OK, 
                        Message = "Thêm thành phố thành công" 
                    });
                }
                return ValidationProblem(ModelState);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RemoveSoftCity (int id)
        {
            try
            {
                var result = await _cityService.RemoveSoftCityAsync(id);
                if(result)
                {
                    return Ok(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Xóa thành phố thành công"
                    });
                } else
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không có thành phố hợp lệ để xóa"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }
}
