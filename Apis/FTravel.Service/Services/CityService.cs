using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<int> CreateCityAsync(int cityCode, string cityName)
        {
            var city = new City
            {
                Code = cityCode,
                Name = cityName,
                UnsignName = StringUtils.ConvertToUnSign(cityName)
            };
            var result = await _cityRepository.CreateCityAsync(city);
            if (result > 0)
            {
                return result;
            }
            else
            {
                return -1;
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

        //public async Task<CityModel> UpdateCityAsync(CityModel updateCityModel, int id)
        //{
        //    var updateCity = _mapper.Map<City>(updateCityModel);
        //    updateCity.Id = id;
        //    updateCity.UnsignName = StringUtils.ConvertToUnSign(updateCityModel.Name);
        //    var result = await _cityRepository.UpdateCityAsync(updateCity);
        //    if(result != null)
        //    {
        //        return updateCityModel;
        //    }
        //    return null;
        //}
        public async Task<bool> RemoveSoftCityAsync(int deleteCity)
        {
            return await _cityRepository.RemoveSoftCityAsync(deleteCity);
        }

    }
}
