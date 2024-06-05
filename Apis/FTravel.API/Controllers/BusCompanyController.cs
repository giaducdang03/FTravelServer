using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
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

        private readonly IBusCompany _busCompanyService;

        public BusCompanyController(IBusCompany busCompanyService)
        {
            _busCompanyService = busCompanyService;
        }

        [HttpGet("get-by-id")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
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

        [HttpGet("get-all-bus-company")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
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
    }
}
