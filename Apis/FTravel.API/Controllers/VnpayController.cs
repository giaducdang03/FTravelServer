using FTravel.API.ViewModels.ResponseModels;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FTravel.API.Controllers
{
    [Route("api/vnpay")]
    [ApiController]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpayService _vnpayService;

        public VnpayController(IVnpayService vnpayService) 
        {
            _vnpayService = vnpayService;    
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment(OrderModel order)
        {
            try
            {
                var paymentUrl = _vnpayService.CreatePaymentUrl(order, HttpContext, "Nap tien FToken");
                VnpayResponseModel responseModel = new VnpayResponseModel() 
                { 
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Ok roi do",
                    ReturnUrl = paymentUrl
                };
                return Ok(responseModel);
            }
            catch (Exception ex) 
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("vnpay-return")]
        public async Task<IActionResult> ConfirmPayment(VnPayModel vnPayModel)
        {
            try
            {
                if (vnPayModel != null) 
                {
                    if (vnPayModel.vnp_TransactionStatus == "00")
                    {

                        return Ok(new ResponseModel()
                        {
                            HttpCode = StatusCodes.Status200OK,
                            Message = "Ok roi do",
                        });
                    }

                    return BadRequest(new ResponseModel()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "7 bai roi",
                    });
                }
                return BadRequest(new ResponseModel()
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    Message = "7 bai roi",
                });
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }
    }
}
