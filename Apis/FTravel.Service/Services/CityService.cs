using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        private readonly IMapper _mapper;
        public CityService(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<City> CreateCityAsync(CityModel cityModel)
        {
           if(cityModel == null)
            {
                return null;

            }
            var city = _mapper.Map<City>(cityModel);
            var result = await _cityRepository.AddAsync(city);
            if (result != null)
            {
                return city;
            } else
            {
                return null;
            }
        }

        public async Task<Pagination<CityModel>> GetListCityAsync(PaginationParameter paginationParameter)
        {
            var listCity = await _cityRepository.GetListCityAsync(paginationParameter);
            if (!listCity.Any())
            {
                return null;
            }
            var listCityModels = _mapper.Map<List<CityModel>>(listCity);
            return new Pagination<CityModel>(listCityModels,
                listCity.TotalCount,
                listCity.CurrentPage,
                listCity.PageSize);
        }

        public async Task<CityModel> UpdateCityAsync(CityModel cityModel)
        {
            var city = _mapper.Map<City>(cityModel);
            var updateCity = await _cityRepository.UpdateCityAsync(city);
            if(updateCity != null)
            {
                return cityModel;
            }
            return null;
        }
        public async Task<bool> RemoveSoftCityAsync(int deleteCity)
        {
           return await _cityRepository.RemoveSoftCityAsync(deleteCity);
        }

    }
}
