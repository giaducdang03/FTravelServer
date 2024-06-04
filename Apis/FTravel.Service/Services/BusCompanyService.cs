using AutoMapper;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class BusCompanyService : IBusCompanyService
    {
        private readonly IBusCompanyRepository _busRepository;
        private readonly IMapper _mapper;
        public BusCompanyService(IBusCompanyRepository repository, IMapper mapper)
        {
            _busRepository = repository;
            _mapper = mapper;
        }
        public async Task<bool> AddBusCompanyAsync(CreateBusCompanyModel model)
        {
            try
            {
                // Map the CreateBusCompanyModel to the appropriate entity model
                var busCompany = _mapper.Map<Repository.EntityModels.BusCompany>(model);

                // Call the repository method to add the bus company asynchronously
                await _busRepository.AddAsync(busCompany);

                // Return true indicating successful addition
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Failed to add bus company: {ex.Message}");

                // Return false indicating failure
                return false;
            }
        }
    }
}
