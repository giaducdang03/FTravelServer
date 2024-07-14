using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
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
        public Task<Pagination<GetAllOrderModel>> GetAllOrderAsync(PaginationParameter paginationParameter, OrderFilter orderFilter);
        public Task<OrderViewModel> GetOrderDetailByIdAsync(int orderId);
        public Task<StatisticRevenueModel> StatisticForDashBoard();

        public Task<ResponseOrderModel> BuyTicketAsync(BuyTicketModel buyTicket, string email);

    }
}
