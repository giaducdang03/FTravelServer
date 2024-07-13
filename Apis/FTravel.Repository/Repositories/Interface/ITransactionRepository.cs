using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        public Task<Pagination<Transaction>> GetTransactionsByWalletId(int walletId, PaginationParameter paginationParameter);
        public Task<Transaction> GetTransactionsByOrderId(int orderId);
    }
}
