using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;

        public WalletService(IWalletRepository walletRepository,
            ICustomerRepository customerRepository,
            ITransactionRepository transactionRepository,
            ISettingService settingService,
            IMapper mapper)
        {
            _walletRepository = walletRepository;
            _customerRepository = customerRepository;
            _transactionRepository = transactionRepository;
            _settingService = settingService;
            _mapper = mapper;
        }
        //public async Task<List<WalletModel>> GetAllWalletsAsync()
        //{
        //    var wallets = await _walletRepository.GetAllAsync();
        //    var walletModels = _mapper.Map<List<WalletModel>>(wallets);
        //    return walletModels;
        //}

        public async Task<Pagination<WalletModel>> GetAllWalletsAsync(PaginationParameter paginationParameter)
        {
            var wallets = await _walletRepository.ToPagination(paginationParameter);
            var customerIds = wallets.Select(w => w.CustomerId).Where(id => id.HasValue).Select(id => id.Value).ToList();

            var customers = await _customerRepository.GetCustomersByIdsAsync(customerIds);

            var walletModels = wallets.Select(wallet =>
            {
                var customer = customers.FirstOrDefault(c => c.Id == wallet.CustomerId);
                var walletModel = _mapper.Map<WalletModel>(wallet);
                if (customer != null)
                {
                    walletModel.CustomerName = customer.FullName;
                }
                return walletModel;
            }).ToList();

            return new Pagination<WalletModel>(walletModels,
                wallets.TotalCount,
                wallets.CurrentPage,
                wallets.PageSize);
        }

        public async Task<WalletModel> GetWalletByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            if (customer != null)
            {
                var customerWallet = await _walletRepository.GetWalletByCustomerId(customer.Id);
                WalletModel walletModel = _mapper.Map<WalletModel>(customerWallet);
                walletModel.CustomerName = customer.FullName;
                return walletModel;
            }
            return null;
        }

        public async Task<string> RequestRechargeIntoWallet(string customerEmail, int amountRecharge, HttpContext context)
        {
            using (var dbTransaction = await _walletRepository.BeginTransactionAsync())
            {
                try
                {
                    if (amountRecharge > 5000)
                    {
                        throw new Exception("You can only recharge a maximum of 5000 FTokens.");
                    }

                    var customer = await _customerRepository.GetCustomerByEmailAsync(customerEmail);
                    if (customer == null)
                    {
                        return null;
                    }
                    var customerWallet = await _walletRepository.GetWalletByCustomerId(customer.Id);
                    if (customerWallet == null)
                    {
                        return null;
                    }

                    // create new transaction

                    Transaction newTransaction = new Transaction()
                    {
                        Amount = amountRecharge,
                        WalletId = customerWallet.Id,
                        TransactionType = TransactionType.IN.ToString(),
                        Status = TransactionStatus.PENDING.ToString(),
                        Description = $"Nap FToken vao vi {customer.UnsignFullName} tu VNPAY"
                    };

                    var transactionAdded = await _transactionRepository.AddAsync(newTransaction);

                    // create URL payment

                    IConfiguration _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

                    DateTime timeNow = DateTime.UtcNow.AddHours(7);

                    // get ftoken value
                    // default = 1
                    int ftokenValue = 1;
                    _ = int.TryParse(await _settingService.GetValueByKeyAsync("FTokenValue"), out ftokenValue);

                    int paymentPrice = amountRecharge * ftokenValue;

                    var ipAddress = VnPayUtils.GetIpAddress(context);

                    var pay = new VnPayLibrary();

                    pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
                    pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
                    pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
                    pay.AddRequestData("vnp_Amount", ((int)paymentPrice * 100).ToString());
                    pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                    pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
                    pay.AddRequestData("vnp_IpAddr", ipAddress);
                    pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
                    pay.AddRequestData("vnp_OrderInfo", transactionAdded.Description);
                    pay.AddRequestData("vnp_OrderType", "250000");
                    pay.AddRequestData("vnp_TxnRef", transactionAdded.Id.ToString());

                    // check server running
                    if (ipAddress == "::1")
                    {
                        pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:UrlReturnLocal"]);
                    }
                    else
                    {
                        pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:UrlReturnAzure"]);
                    }

                    var paymentUrl =
                        pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

                    await dbTransaction.CommitAsync();
                    return paymentUrl;
                }
                catch
                {
                    await dbTransaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> RechargeIntoWallet(VnPayModel vnPayResponse)
        {
            using (var dbTransaction = await _walletRepository.BeginTransactionAsync())
            {
                try
                {
                    IConfiguration _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

                    // get info transaction
                    if (vnPayResponse != null)
                    {
                        // check signature
                        var vnpay = new VnPayLibrary();

                        // get all data from vnpay model
                        foreach (PropertyInfo prop in vnPayResponse.GetType().GetProperties())
                        {
                            string name = prop.Name;
                            object value = prop.GetValue(vnPayResponse, null);
                            string valueStr = value?.ToString() ?? string.Empty;
                            vnpay.AddResponseData(name, valueStr);
                        }

                        var vnpayHashSecret = _configuration["Vnpay:HashSecret"];
                        bool validateSignature = vnpay.ValidateSignature(vnPayResponse.vnp_SecureHash, vnpayHashSecret);

                        int transactionId = 0;
                        _ = int.TryParse(vnPayResponse.vnp_TxnRef, out transactionId);
                        var updateTransaction = await _transactionRepository.GetByIdAsync(transactionId);

                        if (updateTransaction != null)
                        {
                            if (validateSignature)
                            {
                                if (vnPayResponse.vnp_TransactionStatus == "00")
                                {
                                    updateTransaction.Status = TransactionStatus.SUCCESS.ToString();
                                    updateTransaction.TransactionDate = DateTime.ParseExact(vnPayResponse.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                                    // update wallet account balance
                                    var updateWallet = await _walletRepository.GetWalletByIdAsync(updateTransaction.WalletId.Value);
                                    if (updateWallet != null)
                                    {
                                        updateWallet.AccountBalance += updateTransaction.Amount;

                                        await _walletRepository.UpdateAsync(updateWallet);
                                    }

                                    await _transactionRepository.UpdateAsync(updateTransaction);

                                    await dbTransaction.CommitAsync();
                                    return true;
                                }
                                else
                                {
                                    updateTransaction.Status = TransactionStatus.FAILED.ToString();
                                    updateTransaction.TransactionDate = DateTime.ParseExact(vnPayResponse.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                                    await _transactionRepository.UpdateAsync(updateTransaction);

                                    await dbTransaction.CommitAsync();
                                    return false;
                                }
                            }
                            else
                            {
                                updateTransaction.Status = TransactionStatus.FAILED.ToString();
                                updateTransaction.TransactionDate = DateTime.ParseExact(vnPayResponse.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                                await _transactionRepository.UpdateAsync(updateTransaction);

                                await dbTransaction.CommitAsync();
                                return false;
                            }

                        }
                        return false;
                    }
                    return false;
                }
                catch
                {
                    await dbTransaction.RollbackAsync();
                    throw;
                }
            }

        }
    }
}
