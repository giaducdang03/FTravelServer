﻿using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface ITicketTypeService
    {
        public Task<Pagination<TicketTypeModel>> GetAllTicketType(PaginationParameter paginationParameter);

        public Task<TicketTypeModel> GetTicketTypeById(int id);
    }
}