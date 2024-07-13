using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.BuscompanyModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/buscompany")]
    [ApiController]
    public class BusCompanyController : ControllerBase
    {
        private readonly IBusCompanyService _busCompanyService;

        public BusCompanyController(IBusCompanyService busCompanyService)
        {
            _busCompanyService = busCompanyService;
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateBusCompany(CreateBusCompanyModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isAdded = await _busCompanyService.AddBusCompanyAsync(model);

                if (isAdded)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status201Created,
                        Message = "Tạo nhà xe mới thành công!"
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Lỗi xảy ra khi tạo nhà xe!"
                    });
                }
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


        [HttpGet("{id}")]
        [Authorize]
        [Authorize]
        public async Task<IActionResult> GetBusCompanyDetailById(int id)
        {
            try
            {
                var result = await _busCompanyService.GetBusCompanyById(id);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No bus companies was found"
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

        [HttpGet]
        [Authorize]
        [Authorize]
        public async Task<IActionResult> GetAllBusCompanies([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _busCompanyService.GetAllBusCompanies(paginationParameter);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "No bus companies was found"
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> DeleteBusCompany(int id)
        {
            try
            {
                var isDeleted = await _busCompanyService.BusCompanySoftDelete(id);

                if (isDeleted > 0)
                {
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Xóa nhà xe thành công!"
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = "Lỗi xảy ra khi xóa nhà xe!"
                    });
                }
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

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> UpdateBusCompany(int id, UpdateBusCompanyModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isUpdated = await _busCompanyService.UpdateBusCompanyAsync(id, model);

                if (isUpdated)
                {
                    // Return a success response
                    return Ok(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Cập nhật nhà xe thành công"
                    });
                }
                else
                {
                    // Return a not found response if the service was not updated successfully
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Cập nhật nhà xe thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                // Return a bad request response for any other exceptions
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }


    }
}
