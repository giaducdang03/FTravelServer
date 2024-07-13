using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Service.BusinessModels.OrderModels;
using FTravel.Service.Enums;
using FTravel.Service.Services;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

namespace FTravel.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ITicketService _ticketService;
        private readonly IClaimsService _claimsService;

        public OrdersController(IOrderService orderService, ITicketService ticketService, IClaimsService claimsService)
        {
            _orderService = orderService;
            _ticketService = ticketService;
            _claimsService = claimsService;
        }


        //[HttpPost]
        //public async Task<IActionResult> BuyTicket(BuyTicketModel model)
        //{
        //    try
        //    {
        //        var result = await _ticketService.BuyTicketAsync(model);
        //        if (result == null)
        //        {
        //            return NotFound(new ResponseModel
        //            {
        //                HttpCode = StatusCodes.Status404NotFound,
        //                Message = "Lỗi khi mua vé"
        //            });
        //        }

        //        var result2 = await _orderService.CreateOrderAsync(result);
        //        if (result2 != null)
        //        {
        //            return Ok(result2);
        //        }
        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = StatusCodes.Status400BadRequest,
        //            Message = "Lỗi khi tạo đơn"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = 400,
        //            Message = ex.Message
        //        });
        //    }
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BuyTicket(BuyTicketModel model)
        {
            try
            {
                var email = _claimsService.GetCurrentUserEmail;
                var result = await _orderService.BuyTicketAsync(model, email);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Lỗi khi mua vé"
                    });
                }
                return Ok(result);
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


        //[HttpPost("create-order")]
        ////[Authorize(Roles = "ADMIN")]
        //public async Task<IActionResult> CreateNewOrder(OrderModel orderModel)
        //{
        //    try
        //    {
        //        var result = await _orderService.CreateOrderAsync(orderModel);
        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }
        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = StatusCodes.Status400BadRequest,
        //            Message = "Can not create order"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = 400,
        //            Message = ex.Message
        //        });
        //    }
        //}

        //[HttpPost("payment-order")]
        //[Authorize(Roles = "ADMIN")]
        //public async Task<IActionResult> PaymentOrder(int orderId)
        //{
        //    try
        //    {
        //        var result = await _orderService.PaymentOrderAsync(orderId);
        //        if (result == PaymentOrderStatus.SUCCESS)
        //        {
        //            return Ok(new ResponseModel
        //            {
        //                HttpCode = StatusCodes.Status200OK,
        //                Message = "Payment order successfully"
        //            });
        //        }
        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = StatusCodes.Status400BadRequest,
        //            Message = "Can not payment order"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel
        //        {
        //            HttpCode = 400,
        //            Message = ex.Message
        //        });
        //    }
        //}

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrder([FromRoute] PaginationParameter paginationParameter, [FromRoute] OrderFilter orderFilter)
        {
            try
            {
                var result = await _orderService.GetAllOrderAsync(paginationParameter, orderFilter);
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
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không có đơn hàng"
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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetailsById([FromRoute] int id)
        {
            try
            {
                var result = await _orderService.GetOrderDetailByIdAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không có đơn hàng để xem chi tiết"
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

        [HttpGet("statistic")]
        [Authorize(Roles = "ADMIN, BUSCOMPANY")]
        public async Task<IActionResult> StatisticForDashboard()
        {
            try
            {
                var result = await _orderService.StatisticForDashBoard();
                if(result == null)
                {
                    return NotFound(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không có đơn hàng để thống kê"
                    });
                }
                return Ok(result);
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
