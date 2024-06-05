using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;

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
                var busCompany = _mapper.Map<BusCompany>(model);

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


        public async Task<Pagination<BusCompany>> GetAllBusCompanies(PaginationParameter paginationParameter)
        {

            return await _busRepository.GetAllBusCompanies(paginationParameter);
        }

        public async Task<BusCompany> GetBusCompanyById(int id)
        {
            return await _busRepository.GetBusCompanyDetailById(id);
        }
    }
}
