﻿using FTravel.Repository.DBContext;
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

        public async Task<List<string>> GetListOfUser()
        {
            var db = await _context.Users.ToListAsync();
            var fullNames = db.Select(x => x.FullName).ToList();
            return fullNames;
        }

        public async Task<User> GetUserInfoByEmail(string email)
        {
            var data = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
            return data;
        }
    }
}
