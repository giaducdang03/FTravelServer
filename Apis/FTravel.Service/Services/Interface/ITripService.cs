﻿using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface ITripService
    {
        public Task<Pagination<Trip>> GetAllTrips(PaginationParameter paginationParameter);

        public Task<Trip> GetTripDetailById(int id);
    }
}
