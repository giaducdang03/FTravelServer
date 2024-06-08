using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IWalletService
    {
        public Task<Pagination<WalletModel>> GetAllWalletsAsync(PaginationParameter paginationParameter);

        public Task<WalletModel> GetWalletByEmailAsync(string email);

        public Task<string> RequestRechargeIntoWallet(string customerEmail, int amountRecharge, HttpContext context);

        public Task<bool> RechargeIntoWallet(VnPayModel vnPayResponse);

        public Task<Wallet> GetWalletByCustomerIdAsync(int customerId);
    }
}
