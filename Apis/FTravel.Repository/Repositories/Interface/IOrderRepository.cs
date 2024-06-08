using FTravel.Repository.EntityModels;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
