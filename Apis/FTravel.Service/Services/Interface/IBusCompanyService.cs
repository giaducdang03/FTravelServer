using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.BuscompanyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IBusCompanyService
    {
        public Task<bool> AddBusCompanyAsync(CreateBusCompanyModel model);

        public Task<Pagination<BusCompany>> GetAllBusCompanies(PaginationParameter paginationParameter);

        public Task<BusCompany> GetBusCompanyById(int id);

        public Task<int> BusCompanySoftDelete(int busCompanyId);

        public Task<bool> UpdateBusCompanyAsync(int id, UpdateBusCompanyModel busCompany);
    }
}
