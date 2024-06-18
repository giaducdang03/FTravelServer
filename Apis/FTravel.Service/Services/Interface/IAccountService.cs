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
    public interface IAccountService
    {
        Task<Pagination<AccountModel>> GetAllUsersAsync(PaginationParameter paginationParameter);
        Task<User> GetAccountInfoByEmail(string email);

        Task<User> GetAccountInfoById(int id);

        Task<User> BanAccount(int id);

        Task<bool> UpdateAccount(int id,UpdateAccountModel accountModel);

        Task<bool> CreateAccountAsync(CreateAccountModel model);

        public Task<bool> UpdateFcmTokenAsync(string email, string fcmToken);

        public Task<bool> DeleteAccountAsync(int id);
    }
}
