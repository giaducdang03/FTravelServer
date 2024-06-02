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
    public class OtpRepository : GenericRepository<Otp>, IOtpRepository
    {
        private readonly FtravelContext _context;

        public OtpRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Otp> GetOtpByCode(string otpCodde)
        {
            return await _context.Otps.FirstOrDefaultAsync(x => x.OtpCode == otpCodde);
        }
    }
}
