using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface ITransactionService
    {
        public Task<Pagination<TransactionModel>> GetTransactionsByWalletIdAsync(int walletId, PaginationParameter paginationParameter);

        public Task<Transaction> CreateTransactionAsync(Transaction transaction, int customerId);

        public Task<Transaction> GetTransactionByIdAsync(int transactionId);
    }
}
