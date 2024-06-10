using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly FtravelContext _context;

        public CityRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pagination<City>> GetListCityAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Cities.CountAsync();
            var items = await _context.Cities.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<City>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<bool> RemoveSoftCityAsync(int deleteCity)
        {
            var removeSoftCity = await _context.Cities.FirstOrDefaultAsync(x => x.Id == deleteCity);    
            if(removeSoftCity != null)
            {
                if(removeSoftCity.IsDeleted == false)
                {
                    removeSoftCity.IsDeleted = true;
                    _context.SaveChanges();
					return true;
				}
				else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }

        public async Task<City> UpdateCityAsync(City city)
        {
            var cityUpdate =  await _context.Cities.FirstOrDefaultAsync(x => x.Id == city.Id); 
            if (cityUpdate != null)
            {
                cityUpdate.Name = city.Name;
                cityUpdate.UpdateDate = city.UpdateDate;
                cityUpdate.IsDeleted = city.IsDeleted;
                cityUpdate.UnsignName = city.UnsignName;
               await _context.SaveChangesAsync();
               return cityUpdate;

            }
            return null;
        }




    }
}
