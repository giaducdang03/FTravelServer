//using FTravel.API.ViewModels.ResponseModels;
//using FTravel.Service.BusinessModels;
//using FTravel.Service.Services;
//using FTravel.Service.Services.Interface;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel;

//namespace FTravel.API.Controllers
//{
//    [Route("api/orders")]
//    [ApiController]
//    public class OrdersController : ControllerBase
//    {
//        private readonly IOrderService _orderService;

//        public OrdersController(IOrderService orderService) 
//        {
//            _orderService = orderService;
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateNewOrder(OrderModel orderModel)
//        {
//            try
//            {
//                var result = await _orderService.CreateOrderAsync(orderModel);
//                if (result != null) 
//                { 
//                    return Ok(result);
//                }
//                return BadRequest(new ResponseModel
//                {
//                    HttpCode = StatusCodes.Status400BadRequest,
//                    Message = "Can not create order"
//                });
//            }
//            catch (Exception ex) 
//            {
//                return BadRequest(new ResponseModel
//                {
//                    HttpCode = 400,
//                    Message = ex.Message
//                });
//            }
//        }
//    }
//}
