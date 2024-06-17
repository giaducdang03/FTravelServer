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
    public class BusCompanyRepository : GenericRepository<BusCompany>, IBusCompanyRepository
    {
        private readonly FtravelContext _context;

        public BusCompanyRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pagination<BusCompany>> GetAllBusCompanies(PaginationParameter paginationParameter)
        {

            var itemCount = await _context.BusCompanies.CountAsync();
            var items = await _context.BusCompanies.OrderBy(x => x.Name).Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<BusCompany>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }

        public async Task<BusCompany> GetBusCompanyByManagerEmail(string email)
        {
            return await _context.BusCompanies.FirstOrDefaultAsync(x => x.ManagerEmail == email);
        }

        public Task<BusCompany> GetBusCompanyDetailById(int id)
        {

            return _context.BusCompanies.FirstOrDefaultAsync(x => x.Id == id);
        }


    }
}
