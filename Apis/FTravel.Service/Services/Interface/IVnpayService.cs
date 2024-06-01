using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IVnpayService
    {
        string CreatePaymentUrl(OrderModel orderModel, HttpContext context, string vnp_OrderInfo);
    }
}
