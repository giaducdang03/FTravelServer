using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{

    public class BusCompanyService : IBusCompany
    {
        private readonly IBusCompanyRepository _busCompanyRepository;

        public BusCompanyService(IBusCompanyRepository busCompanyRepository)
        {
            _busCompanyRepository = busCompanyRepository;
        }



        public async Task<Pagination<BusCompany>> GetAllBusCompanies(PaginationParameter paginationParameter)
        {

            return await _busCompanyRepository.GetAllBusCompanies(paginationParameter);
        }

        public async Task<BusCompany> GetBusCompanyById(int id)
        {
            return await _busCompanyRepository.GetBusCompanyDetailById(id);
        }
    }
}
