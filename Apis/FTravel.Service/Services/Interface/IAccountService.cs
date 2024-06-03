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
        Task<List<AccountModel>> GetAllUserAscyn();
        Task<User> GetAccountInfoByEmail(string email);

        Task<AccountModel> CreateAccount(AccountModel account);
    }
}
