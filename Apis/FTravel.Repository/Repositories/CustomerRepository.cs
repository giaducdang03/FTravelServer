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
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly FtravelContext _context;

        public CustomerRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email) 
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
