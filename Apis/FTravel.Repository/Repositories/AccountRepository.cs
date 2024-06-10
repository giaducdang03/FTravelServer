using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class AccountRepository : GenericRepository<User>, IAccountRepository
    {
        private readonly FtravelContext _context;
        public AccountRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> CreateAccount(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUser()
        {
            var db = await _context.Users.ToListAsync();
            return db;
        }

        public async Task<Pagination<User>> GetAllUserAccount(PaginationParameter paginationParameter)
        {
            var query = _context.Users.AsQueryable();

            var totalCount = await query.CountAsync();
            var paginatedQuery = query.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                      .Take(paginationParameter.PageSize);

            var account = await paginatedQuery.ToListAsync();

            return new Pagination<User>(account, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
        }

        public async Task<List<string>> GetListOfUser()
        {
            var db = await _context.Users.ToListAsync();
            var fullNames = db.Select(x => x.FullName).ToList();
            return fullNames;
        }

        public async Task<User> GetUserInfoByEmail(string email)
        {
            var data = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
            var userWithId = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));

            if (userWithId != null)
            {
                data.Id = userWithId.Id;
            }
            return data;
        }

        public async Task<User> GetUserInfoById(int id)
        {
            var data = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
            var userWithId = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (userWithId != null)
            {
                data.Id = userWithId.Id;
            }
            return data;
        }
    }
}
