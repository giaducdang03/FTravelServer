using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.TicketModels;
using FTravel.Service.Services.Interface;
using FTravel.Repository.Repositories.Interface;
using AutoMapper;

namespace FTravel.Service.Services
{
    public class TicketService : ITicketService
    {
        private ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<TicketModel> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetTripDetailById(id);
            TicketModel ticketModel = _mapper.Map<TicketModel>(ticket);
            return ticketModel;
        }
    }
}
