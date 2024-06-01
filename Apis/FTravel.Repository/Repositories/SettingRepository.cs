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
    public class SettingRepository : GenericRepository<Setting>, ISettingRepository
    {
        private readonly FtravelContext _context;

        public SettingRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Setting> GetByKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var loadSetting = await _context.Settings.Where(x => x.Key == key).FirstOrDefaultAsync();
                return loadSetting;
            }
            return null;
        }
    }
}
