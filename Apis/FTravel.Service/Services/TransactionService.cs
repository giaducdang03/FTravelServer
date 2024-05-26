using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
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
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository,
            IMapper mapper) 
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
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
    }
}
