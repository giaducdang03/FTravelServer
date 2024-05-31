using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        public TripService(ITripRepository repository, ITicketRepository ticketRepository, IMapper mapper)
        {
            _tripRepository = repository;
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }
        public async Task<Pagination<TripModel>> GetAllAsync(PaginationParameter paginationParameter)
        {
            var trips = await _tripRepository.GetAll(paginationParameter);
            if (!trips.Any())
            {
                return null;
            }
            var tripModels = _mapper.Map<List<TripModel>>(trips);

            return new Pagination<TripModel>(tripModels,
                trips.TotalCount,
                trips.CurrentPage,
                trips.PageSize);
        }

        public async Task<TripModel> GetTripByIdAsync(int id)
        {
            var trip = await _tripRepository.GetTripById(id);
            var tickets = await _ticketRepository.GetAllByTripId(id);
            if (trip == null)
            {
                return null;
            }
            var tripModel = _mapper.Map<TripModel>(trip);
            tripModel.Tickets = _mapper.Map<List<TicketModel>>(tickets);
            return tripModel;
        }
    }
}
