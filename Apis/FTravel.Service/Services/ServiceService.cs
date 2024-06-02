﻿using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
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
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        public ServiceService(IServiceRepository repository, IMapper mapper )
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
            var services = await _serviceRepository.GetAll( paginationParameter);
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
                // Log the exception
                Console.WriteLine($"Fail to add service {ex.Message}");
                return false; // Return false indicating failure
            }
        }
    }
}