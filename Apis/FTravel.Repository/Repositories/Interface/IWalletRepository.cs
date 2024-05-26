using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IWalletRepository: IGenericRepository<Wallet>
    {
        public Task<Wallet> GetWalletByCustomerId(int customerId);

        public Task<Wallet> GetWalletByIdAsync(int walletId);
    }
}
