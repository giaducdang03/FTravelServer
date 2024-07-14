using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly FtravelContext _context;

        public TransactionRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pagination<Transaction>> GetTransactionsByWalletId(int walletId, PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Transactions.CountAsync();
            var items = await _context.Transactions.Where(x => x.WalletId == walletId).OrderByDescending(x => x.TransactionDate).Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Transaction>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<Transaction> GetTransactionsByOrderId(int orderId)
        {
            var result = await _context.Transactions.FirstOrDefaultAsync(x => x.OrderId == orderId);
            return result;
        }

    }
}
