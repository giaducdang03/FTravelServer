using AutoMapper;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels.OrderModels;
using FTravel.Service.BusinessModels.RouteModels;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IServiceTicketRepository _serviceTicketRepository;

        public OrderService(IOrderRepository orderRepository,
            ITransactionService transactionService,
            IWalletService walletService,
            IBusCompanyRepository busCompanyRepository,
            ICustomerRepository customerRepository,
            IMapper mapper,
            ITicketRepository ticketRepository,
            ITripRepository tripRepository,
            IServiceTicketRepository serviceTicketRepository)
        {
            _orderRepository = orderRepository;
            _transactionService = transactionService;
            _walletService = walletService;
            _busCompanyRepository = busCompanyRepository;
            _customerRepository = customerRepository;
            _ticketRepository = ticketRepository;
            _tripRepository = tripRepository;
            _serviceTicketRepository = serviceTicketRepository;
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
            if (listOrders.Count > 0)
            {
                return listOrders;
            }
            return null;
        }
        public async Task<OrderViewModel> GetOrderDetailByIdAsync(int orderId)
        {
            var findOrderDetail = await _orderRepository.GetOrderDetailByIdAsync(orderId);
            var findOrder = await _orderRepository.GetByIdAsync(orderId);
            if (findOrderDetail == null)
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
                OrderDetailModel = _mapper.Map<List<OrderDetailModel>>(findOrderDetail),
            };
            return result;
        }
        public async Task<StatisticRevenueModel> StatisticForDashBoard()
        {
            var listOrder = await _orderRepository.StatisticForDashBoard();
            var totalUser = await _customerRepository.GetAllAsync();
            var chartOrders = listOrder
                            .GroupBy(o => o.Order.CreateDate.Year)
                            .Select(g => new ChartOrderModel
                            {
                                Year = g.Key,
                                TicketBooked = g.Count(o => !o.IsDeleted),
                                TicketCancel = g.Count(o => o.IsDeleted)
                            })
                            .ToList();
            var getTimeLine = listOrder
                            .Select(od => new TimeLineModel
                            {
                                CreateDate = od.Order.CreateDate,
                                StartPoint = od.Ticket.Trip.Route.StartPointNavigation.Name,
                                EndPoint = od.Ticket.Trip.Route.EndPointNavigation.Name,
                                BusCompanyName = od.Ticket.Trip.Route.BusCompany.Name,
                                Name = od.Ticket.Trip.Route.Name
                            })
                            .OrderByDescending(od => od.CreateDate)
                            .ToList();

            var result = new StatisticRevenueModel()
            {
                TotalPrice = listOrder.Sum(x => x.Order.TotalPrice),
                AmountOfUser = totalUser.Count,
                AmountOfOrder = listOrder.Select(x => x.Order).Count(),
                ChartOrders = chartOrders,
                TimeLine = getTimeLine
            };
            return result;
        }

        public async Task<ResponseOrderModel> BuyTicketAsync(BuyTicketModel buyTicketModel)
        {
            var ticketBuy = await _ticketRepository.GetTicketByIdAsync(buyTicketModel.TicketId);

            if (ticketBuy != null)
            {
                if (ticketBuy.Status == TicketStatus.SOLD.ToString()) 
                {
                    throw new Exception("Vé này đã được bán.");
                }

                var customer = await _customerRepository.GetByIdAsync(buyTicketModel.CustomerId);

                if (customer == null)
                {
                    throw new Exception("Người dùng không tồn tại.");
                }

                using (var orderTransaction = await _orderRepository.BeginTransactionAsync())
                {
                    try
                    {
                        var trip = await _tripRepository.GetTripById(ticketBuy.TripId.Value);

                        if (trip != null)
                        {
                            if (trip.IsDeleted)
                            {
                                throw new Exception("Chuyến không tồn tại hoặc đã bị xóa.");
                            }

                            var listServiceOnTrip = await _tripRepository.GetServiceByTripId(trip.Id);
                            if (listServiceOnTrip.Any())
                            {
                                int totalPrice = 0;

                                if (buyTicketModel.Services.Any())
                                {
                                    bool checkServices = listServiceOnTrip.Any(item1 => buyTicketModel.Services.Any(item2 => item1.ServiceId == item2.Id));
                                    if (checkServices)
                                    {
                                        var buyServices = listServiceOnTrip.Where(item1 => buyTicketModel.Services.Any(item2 => item1.ServiceId == item2.Id)).ToList();

                                        var serviceTickets = new List<ServiceTicket>();

                                        foreach (var serviceItem in buyServices)
                                        {
                                            //var quantityService = buyTicketModel.Services.First(x => x.Id == serviceItem.Id).Quantity;
                                            var quantityService = buyTicketModel.Services.First(x => x.Id == serviceItem.ServiceId)?.Quantity ?? 0;
                                            totalPrice += (serviceItem.ServicePrice ?? 0) * quantityService;
                                            var newSerivceTicket = new ServiceTicket
                                            {
                                                ServiceId = serviceItem.ServiceId,
                                                TicketId = ticketBuy.Id,
                                                Price = serviceItem.ServicePrice,
                                                Quantity = quantityService
                                            };
                                            serviceTickets.Add(newSerivceTicket);
                                        }

                                        await _serviceTicketRepository.AddRangeAsync(serviceTickets);
                                    }

                                    var orderDetails = new List<OrderDetail>();

                                    var newOrderDetail = new OrderDetail
                                    {
                                        TicketId = ticketBuy.Id,
                                        Type = "Ticket",
                                        UnitPrice = totalPrice + ticketBuy.TicketType.Price,
                                        Quantity = 1
                                    };
                                    orderDetails.Add(newOrderDetail);

                                    //var newOrder = new OrderModel
                                    //{
                                    //    TotalPrice = totalPrice,
                                    //    PaymentStatus = TransactionStatus.PENDING,
                                    //    CustomerId = customer.Id,
                                    //    OrderDetails = orderDetails
                                    //};

                                    // create new order
                                    var orderCode = GenerateOrderCode();
                                    var newOrder = new Order()
                                    {
                                        Code = orderCode,
                                        TotalPrice = totalPrice + ticketBuy.TicketType.Price,
                                        CustomerId = customer.Id,
                                        PaymentStatus = TransactionStatus.PENDING.ToString(),
                                        OrderDetails = orderDetails,
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
                                            Amount = addedOrder.TotalPrice.Value,
                                        };

                                        var addedTransaction = await _transactionService.CreateTransactionAsync(newTransaction, addedOrder.CustomerId.Value);

                                        if (addedTransaction != null)
                                        {
                                            // payment
                                            PaymentOrderStatus checkPayment = await PaymentOrderAsync(addedOrder.Id);
                                            if (checkPayment == PaymentOrderStatus.SUCCESS)
                                            {
                                                // update ticket
                                                ticketBuy.Status = TicketStatus.SOLD.ToString();
                                                await _ticketRepository.UpdateAsync(ticketBuy);
                                            }
                                            else
                                            {
                                                var insertServiceTickets = await _serviceTicketRepository.GetServiceTicketByTicketId(ticketBuy.Id);
                                                if (insertServiceTickets.Any())
                                                {
                                                    await _serviceTicketRepository.PermanentDeletedListAsync(insertServiceTickets);
                                                }
                                            }

                                            await orderTransaction.CommitAsync();
                                            return _mapper.Map<ResponseOrderModel>(newOrder);
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    throw new Exception("Danh sách dịch vụ không hợp lệ.");
                                }
                            }
                            else
                            {
                                throw new Exception("Chuyến đi không có dịch vụ.");
                            }
                        }
                    }
                    catch
                    {
                        await orderTransaction.RollbackAsync();
                        throw;
                    }
                }

            }
            throw new Exception("Vé không hợp lệ.");
        }
    }
}
