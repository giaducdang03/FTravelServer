﻿using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IRouteService
    {
        public Task<Pagination<RouteModel>> GetListRouteAsync(PaginationParameter paginationParameter);
        public Task<RouteModel?> GetRouteDetailByRouteIdAsync(int routeId);
    }
}