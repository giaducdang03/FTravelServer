using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
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
        private readonly IRouteRepository _routeRepository;
        private readonly IMapper _mapper;
        public TripService(ITripRepository repository, ITicketRepository ticketRepository, IRouteRepository routeRepository, IMapper mapper)
        {
            _tripRepository = repository;
            _ticketRepository = ticketRepository;
            _routeRepository = routeRepository;
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
        public async Task<bool> CreateTripAsync(CreateTripModel tripModel)
        {
            try
            {
                var route = await _routeRepository.GetRouteDetailByRouteIdAsync(tripModel.RouteId);
                if (route == null) 
                {
                    Console.WriteLine($"Route not found for ID: {tripModel.RouteId}");
                    return false;
                }

                var newTrip = _mapper.Map<Trip>(tripModel);

                // Add valid newTrip services
                foreach (var tripService in tripModel.TripServices)
                {
                    var service = route.Services.FirstOrDefault(s => s.Id == tripService.ServiceId);
                    if (service != null)
                    {
                        newTrip.TripServices.Add(new Repository.EntityModels.TripService { Service = service,
                            ServicePrice = tripService.Price
                        });
                    }
                }

                foreach (var ticketTypeId in tripModel.TicketTypeIds)
                {
                    var ticketType = route.TicketTypes.FirstOrDefault(t => t.Id == ticketTypeId);
                    if (ticketType != null)
                    {
                        newTrip.TripTicketTypes.Add(new TripTicketType { TicketType = ticketType });
                    }
                }

                // Add newTrip to the repository
                return await _tripRepository.CreateTripAsync(newTrip);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error occurred while creating newTrip: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTripAsync(UpdateTripModel tripModel)
        {
            // Validate status
            if (!Enum.TryParse(typeof(TripStatus), tripModel.Status, true, out _))
            {
                throw new ArgumentException($"Invalid status value. Allowed values are: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}.");
            }

            var existingTrip = await _tripRepository.GetTripById(tripModel.TripId);
            if (existingTrip == null)
            {
                throw new KeyNotFoundException($"Trip With id:{tripModel.TripId}  not found.");
            }
            int routeId = existingTrip.RouteId == null ? default(int) : existingTrip.RouteId.Value;

            var route = await _routeRepository.GetRouteDetailByRouteIdAsync(routeId);
            if (route == null)
            {
                throw new KeyNotFoundException("Route not found.");
            }

            _mapper.Map(tripModel, existingTrip);

            existingTrip.TripServices.Clear();
            foreach (var tripServiceModel in tripModel.TripServices)
            {
                var service = route.Services.FirstOrDefault(s => s.Id == tripServiceModel.ServiceId);
                if (service != null)
                {
                    existingTrip.TripServices.Add(new Repository.EntityModels.TripService { Service = service, ServicePrice = tripServiceModel.Price });
                }
            }

            existingTrip.TripTicketTypes.Clear();
            foreach (var ticketTypeId in tripModel.TicketTypeIds)
            {
                var ticketType = route.TicketTypes.FirstOrDefault(t => t.Id == ticketTypeId);
                if (ticketType != null)
                {
                    existingTrip.TripTicketTypes.Add(new TripTicketType { TicketType = ticketType });
                }
            }

            return await _tripRepository.UpdateTripAsync(existingTrip);
        }
    }
}
