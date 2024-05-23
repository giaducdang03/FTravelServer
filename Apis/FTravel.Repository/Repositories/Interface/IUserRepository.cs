using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User?> GetUserByEmailAsync(string email);
    }
}
