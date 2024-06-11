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
    public interface ICityRepository : IGenericRepository<City>
    {
        public Task<Pagination<City>> GetListCityAsync(PaginationParameter paginationParameter);

        public Task<City> UpdateCityAsync (City updateCity);
        public Task<int> CreateCityAsync (City createCity);

        public Task<bool> RemoveSoftCityAsync (int deleteCity);
    }
}
