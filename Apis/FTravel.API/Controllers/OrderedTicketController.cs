using FTravel.API.ViewModels.ResponseModels;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FTravel.API.Controllers
{
    [Route("api/orderedticket")]
    [ApiController]
    public class OrderedTicketController : ControllerBase
    {
        private readonly IOrderedTicketService _orderedTicketService;

        public OrderedTicketController(IOrderedTicketService orderedTicketService)
        {
            _orderedTicketService = orderedTicketService;
        }

        [HttpGet("by-orderid/{orderid}")]
        public async Task<IActionResult> GetOrderedTicketDetailByOrderId(int orderId)
        {
            try
            {
                var data = await _orderedTicketService.GetAllOrderedTicketDetailByOrderIdService(orderId);
                if (orderId == null)
                {
                    return BadRequest();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel
                {
                    HttpCode = 400,
                    Message = ex.Message
                });
            }

        }

        [HttpGet("by-cutomerid/{cusid}")]
        public async Task<IActionResult> GetOrderedTicketByCustomerId(int customerId)
        {
            try
            {
                var data = await _orderedTicketService.GetAllOrderedTicketByCustomerIdService(customerId);
                if (customerId == null)
                {
                    return BadRequest();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel
                {
                    HttpCode = 400,
                    Message = ex.Message
                });
            }

        }
    }
}
