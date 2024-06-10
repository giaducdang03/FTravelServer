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
        Task<List<AccountModel>> GetAllUserAscyn();
        Task<User> GetAccountInfoByEmail(string email);
        Task<User> GetAccountInfoById(int id);


        //Task<AccountModel> CreateAccount(AccountModel account);
        public Task<Pagination<AccountModel>> GetAllUserAccountService(PaginationParameter paginationParameter);
        Task<bool> CreateAccountAsync(CreateAccountModel model);
    }
}
