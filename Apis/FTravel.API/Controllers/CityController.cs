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
                        Message = "City is empty"
                    });
                }
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(responseModel);
            }
        }

        [HttpPut("{id}")]
        //[Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> UpdateCity([FromBody] CityRequestModel cityRequestModel, [FromRoute] int id)
        {
            try
            {
                var updateCityModel = new CityModel()
                {
                    Id = id,
                    Name = cityRequestModel.Name,
                    Code = cityRequestModel.Code
                };
                var result = await _cityService.UpdateCityAsync(updateCityModel);
                if(result == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not find this City to Update"
                    });
                }
                return Ok(new ResponseModel()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update City success"
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

        [HttpPost]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateCity([FromBody] CityRequestModel cityRequestModel)
		{
            try
            {
                CityModel cityModel = new CityModel()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = null,
                    IsDeleted = false,
                    Name = cityRequestModel.Name,
                    Code = cityRequestModel.Code
				};
                var result = await _cityService.CreateCityAsync(cityModel);
                if (result <= 0)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not add this city"
                    });
                }
                return Ok(new ResponseModel() { HttpCode = StatusCodes.Status200OK, Message = "Add Cities Success" });

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

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> removeSoftCity (int id)
        {
            try
            {
                var result = await _cityService.RemoveSoftCityAsync(id);
                if(result)
                {
                    return Ok(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove soft city success"
                    });
                } else
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "The city does not exist or has been soft deleted"
                    });
                }
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
