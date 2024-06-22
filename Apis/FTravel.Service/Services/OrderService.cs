using AutoMapper;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;
        private readonly IMapper _mapper;
        private readonly IBusCompanyRepository _busCompanyRepository;

        public OrderService(IOrderRepository orderRepository,
            ITransactionService transactionService,
            IWalletService walletService,
            IBusCompanyRepository busCompanyRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _transactionService = transactionService;
            _walletService = walletService;
            _busCompanyRepository = busCompanyRepository;
            _mapper = mapper;
        }

        public async Task<Order> CreateOrderAsync(OrderModel orderModel)
        {
            using (var orderTransaction = await _orderRepository.BeginTransactionAsync())
            {
                try
                {
                    // create new order
                    var orderCode = GenerateOrderCode();
                    var newOrder = new Order()
                    {
                        Code = orderCode,
                        TotalPrice = orderModel.TotalPrice,
                        CustomerId = orderModel.CustomerId,
                        PaymentStatus = orderModel.PaymentStatus.ToString(),
                        OrderDetails = orderModel.OrderDetails,
                    };

                    var addedOrder = await _orderRepository.AddAsync(newOrder);

                    if (addedOrder != null)
                    {
                        // create new transaction

                        Transaction newTransaction = new Transaction()
                        {
                            OrderId = addedOrder.Id,
                            Description = $"Thanh toan cho don hang {orderCode}",
                            TransactionType = TransactionType.OUT.ToString(),
                            Amount = orderModel.TotalPrice,
                        };

                        var addedTransaction = await _transactionService.CreateTransactionAsync(newTransaction, orderModel.CustomerId);

                        if (addedTransaction != null)
                        {
                            await orderTransaction.CommitAsync();
                            return addedOrder;
                        }
                    }

                    await orderTransaction.RollbackAsync();
                    return null;
                }
                catch
                {
                    await orderTransaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<PaymentOrderStatus> PaymentOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                // check payment order
                if (order.PaymentDate != null)
                {
                    return PaymentOrderStatus.FAILED;
                }

                // get transaction
                var transactions = order.Transactions.FirstOrDefault(x => x.TransactionType == TransactionType.OUT.ToString());
                if (transactions != null)
                {
                    // check payment
                    int walletId = transactions.WalletId;
                    int transactionAmount = transactions.Amount;

                    int result = await _walletService.ExecutePaymentAsync(walletId, TransactionType.OUT, transactionAmount, transactions.Id);
                    if (result > 0)
                    {
                        var currentTransaction = await _transactionService.GetTransactionByIdAsync(transactions.Id);
                        if (currentTransaction != null)
                        {
                            // update order
                            order.PaymentDate = currentTransaction.TransactionDate;
                            order.PaymentStatus = currentTransaction.Status;

                            await _orderRepository.UpdateAsync(order);
                            
                            if (currentTransaction.Status == TransactionStatus.SUCCESS.ToString())
                            {
                                return PaymentOrderStatus.SUCCESS;
                            }
                            
                            // wallet not enough account balance
                            return PaymentOrderStatus.NOTPAYMENT;
                        }
                    }
                }
            }
            return PaymentOrderStatus.FAILED;
        }

        private string GenerateOrderCode()
        {
            DateTime now = TimeUtils.GetTimeVietNam();
            string orderCode = now.ToString("yyyyMMddHHmmss");
            return orderCode;
        }
        public async Task<List<OrderViewModel>> GetAllOrderAsync()
        {
            var orderList = await _orderRepository.GetAllOrderAsync();
            List<OrderViewModel> listOrders = _mapper.Map<List<OrderViewModel>>(orderList);
            if(listOrders.Count > 0)
            {
                return listOrders;
            }
            return null;
        }
        public async Task<OrderViewModel> GetOrderDetailByIdAsync(int orderId)
        {
            var findOrderDetail = await _orderRepository.GetOrderDetailByIdAsync(orderId);
            var findOrder = await _orderRepository.GetByIdAsync(orderId);
            if(findOrderDetail == null)
            {
                return null; 
            }
            var result = new OrderViewModel()
            {
                Id = findOrder.Id,
                CreateDate = findOrder.CreateDate,
                UpdateDate = findOrder.UpdateDate,
                IsDeleted = findOrder.IsDeleted,
                TotalPrice = findOrder.TotalPrice,
                Code = findOrder.Code,
                PaymentDate = (DateTime)findOrder.PaymentDate,
                PaymentOrderStatus = findOrder.PaymentStatus.ToString(),
                CustomerName = findOrder.Customer.FullName,
                OrderDetailModel =  _mapper.Map<List<OrderDetailModel>>(findOrderDetail),
            };
            return result;
        }
    }
}
