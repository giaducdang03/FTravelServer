using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.OrderModels;
using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(OrderModel orderModel);

        public Task<PaymentOrderStatus> PaymentOrderAsync(int orderId);
        public Task<List<OrderViewModel>> GetAllOrderAsync();
        public Task<OrderViewModel> GetOrderDetailByIdAsync(int orderId);
        public Task<StatisticRevenueModel> StatisticForDashBoard();



    }
}
