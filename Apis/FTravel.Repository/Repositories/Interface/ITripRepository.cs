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
    public interface ITripRepository : IGenericRepository<Trip>
    {
        public Task<Pagination<Trip>> GetAll(PaginationParameter paginationParameter);
        public Task<Trip> GetTripById(int id);
    }
}