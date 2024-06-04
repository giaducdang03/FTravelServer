using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class BusCompanyRepository :  GenericRepository<BusCompany>, IBusCompanyRepository
    {
        private readonly FtravelContext _context;
        public BusCompanyRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }
    }
}
