using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public Task<Customer?> GetCustomerByEmailAsync(string email);

        public Task<List<Customer>> GetCustomersByIdsAsync(IEnumerable<int> customerIds);
    }
}
