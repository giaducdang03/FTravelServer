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
    public interface IAccountRepository : IGenericRepository<User>
    {
        public Task<List<User>> GetAllUser();

        Task<Pagination<User>> GetAllUserAccount(PaginationParameter paginationParameter);
        public Task<List<string>> GetListOfUser();
        public Task<User> CreateAccount(User user); 
        public Task<User> GetUserInfoByEmail(string email);

        public Task<User> GetUserInfoById(int id);
    }
}
