using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels.ServiceModels;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        public ServiceService(IServiceRepository repository, IMapper mapper)
        {
            _serviceRepository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceModel> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if (service == null)
            {
                return null;
            }
            var serviceModel = _mapper.Map<ServiceModel>(service);
            return serviceModel;
        }

        public async Task<Pagination<ServiceModel>> GetAllAsync(PaginationParameter paginationParameter)
        {
            var services = await _serviceRepository.GetAll(paginationParameter);
            if (!services.Any())
            {
                return null;
            }
            var serviceModels = _mapper.Map<List<ServiceModel>>(services);
            return new Pagination<ServiceModel>(serviceModels,
                services.TotalCount,
                services.CurrentPage,
                services.PageSize);

        }
        public async Task<Pagination<ServiceModel>> GetAllServiceByRouteIdAsync(int routeId, PaginationParameter paginationParameter)
        {
            var services = await _serviceRepository.GetAllServiceByRouteId(routeId, paginationParameter);
            if (!services.Any())
            {
                return null;
            }
            var serviceModels = _mapper.Map<List<ServiceModel>>(services);
            return new Pagination<ServiceModel>(serviceModels,
                services.TotalCount,
                services.CurrentPage,
                services.PageSize);

        }
        public async Task<Pagination<ServiceModel>> GetAllServiceByStationIdAsync(int stationId, PaginationParameter paginationParameter)
        {
            var services = await _serviceRepository.GetAllServiceByStationId(stationId, paginationParameter);
            if (!services.Any())
            {
                return null;
            }
            var serviceModels = _mapper.Map<List<ServiceModel>>(services);
            return new Pagination<ServiceModel>(serviceModels,
                services.TotalCount,
                services.CurrentPage,
                services.PageSize);
        }

        public async Task<bool> AddServiceAsync(CreateServiceModel serviceToCreate)
        {
            try
            {
                var service = _mapper.Map<Repository.EntityModels.Service>(serviceToCreate);
                await _serviceRepository.AddAsync(service);
                return true; // Return true indicating success
            }
            catch (Exception ex)
            {
                throw new Exception("Xảy ra lỗi khi thêm dịch vụ");
            }
        }

        public async Task<bool> UpdateServiceAsync(int id, UpdateServiceModel serviceToUpdate)
        {
            try
            {
                var existingService = await _serviceRepository.GetServiceById(id);
                if (existingService == null)
                {
                    return false;
                }

                _mapper.Map(serviceToUpdate, existingService);
                await _serviceRepository.UpdateAsync(existingService);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Xảy ra lỗi khi cập nhật dịch vụ");
            }
        }
        public async Task<bool> DeleteServiceAsync(int id)
        {
            try
            {
                var existingService = await _serviceRepository.GetServiceById(id);
                if (existingService == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy dịch vụ!");
                    return false;
                }
                await _serviceRepository.SoftDeleteAsync(existingService);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Xảy ra lỗi khi xóa dịch vụ");
            }
        }

    }
}
