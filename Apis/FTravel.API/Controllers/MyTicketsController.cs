using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTravel.API.Controllers
{
    [Route("api/my-tickets")]
    [ApiController]
    public class MyTicketsController : ControllerBase
    {
        private readonly IOrderedTicketService _orderedTicketService;
        private readonly IClaimsService _claimsService;

        public MyTicketsController(IOrderedTicketService orderedTicketService, IClaimsService claimsService) 
        {
            _orderedTicketService = orderedTicketService;
            _claimsService = claimsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyTickets(PaginationParameter paginationParameter)
        {
            try
            {
                var email = _claimsService.GetCurrentUserEmail;
                var result = await _orderedTicketService.GetTicketByCustomer(email, paginationParameter);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Not found tickets"
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
                return BadRequest(new ResponseModel
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{ticketId}")]
        [Authorize]
        public async Task<IActionResult> GetMyTicketsById(int ticketId)
        {
            try
            {
                var result = await _orderedTicketService.GetTicketDetailCustomerById(ticketId);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Not found ticket"
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
    }
}
