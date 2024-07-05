using FTravel.API.ViewModels.ResponseModels;
using FTravel.Service.BusinessModels.OrderModels;
using FTravel.Service.BusinessModels.TicketModels;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FTravel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IOrderService orderService;


        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetTicketById(int id)
        {
            try
            {
                var result = await _ticketService.GetTicketByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không có ticket phù hợp"
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
