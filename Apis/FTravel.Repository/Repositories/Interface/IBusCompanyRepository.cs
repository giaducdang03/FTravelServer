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
    public interface IBusCompanyRepository : IGenericRepository<BusCompany>
    {
        public Task<Pagination<BusCompany>> GetAllBusCompanies(PaginationParameter paginationParameter);

        public Task<BusCompany> GetBusCompanyDetailById(int id);

        public Task<BusCompany> GetBusCompanyByManagerEmail(string email);

        public Task<int> SoftDeleteBusCompany(int busCompanyId);
        
    }
}
