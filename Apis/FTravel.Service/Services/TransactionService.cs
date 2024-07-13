using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels.PaymentModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletService _walletService;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository,
            IWalletService walletService,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _walletService = walletService;
            _mapper = mapper;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction, int customerId)
        {
            var wallet = await _walletService.GetWalletByCustomerIdAsync(customerId);
            if (wallet != null)
            {
                transaction.WalletId = wallet.Id;
                transaction.Status = TransactionStatus.PENDING.ToString();
                return await _transactionRepository.AddAsync(transaction);
            } 
            else
            {
                throw new Exception("Not found wallet customer.");
            }
        }

        public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
        {
            return await _transactionRepository.GetByIdAsync(transactionId);
        }

        public async Task<Pagination<TransactionModel>> GetTransactionsByWalletIdAsync(int walletId, PaginationParameter paginationParameter)
        {
            var transactions = await _transactionRepository.GetTransactionsByWalletId(walletId, paginationParameter);
            if (!transactions.Any())
            {
                return null;
            }
            var transactionModels = _mapper.Map<List<TransactionModel>>(transactions);
            return new Pagination<TransactionModel>(transactionModels,
                transactions.TotalCount,
                transactions.CurrentPage,
                transactions.PageSize);
        }
        public async Task<Transaction> GetTransactionByOrderIdAsync(int orderId)
        {
            return await _transactionRepository.GetTransactionsByOrderId(orderId);
        }

    }
}
