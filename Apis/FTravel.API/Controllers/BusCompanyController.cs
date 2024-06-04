using FTravel.API.ViewModels.ResponseModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                    Message = "Bus company created successfully"
                });
            }
            else
            {
                return BadRequest(
                                   new ResponseModel
                                   {
                                       HttpCode = StatusCodes.Status500InternalServerError,
                                       Message = "Failed to create bus company"
                                   });
            }
        }
    }
}
