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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly FtravelContext _context;

        public RoleRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            var existRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            return existRole;
        }
    }
}
