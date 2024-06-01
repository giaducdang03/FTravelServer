﻿using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IRouteRepository : IGenericRepository<Route>
    {
        public Task<Pagination<Route>> GetListRoutesAsync(PaginationParameter paginationParameter);

        public Task<Route?> GetRouteDetailByRouteIdAsync(int routeId);
    }
}