using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
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

        public async Task<Pagination<TripModel>> GetAllTripAsync(PaginationParameter paginationParameter)
        {
            var trips = await _tripRepository.GetAllTrips(paginationParameter);
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
                if (!Enum.TryParse(typeof(TripStatus), tripModel.Status, true, out _))
                {
                    throw new ArgumentException($"Trạng thái không hợp lệ. Trạng thái có thể là: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}.");
                }
                var route = await _routeRepository.GetRouteDetailByRouteIdAsync(tripModel.RouteId);
                if (route == null)
                {
                    Console.WriteLine($"Không tìm thấy tuyến xe có id: {tripModel.RouteId}");
                    return false;
                }

                var newTrip = _mapper.Map<Trip>(tripModel);

                // Add valid newTrip services
                foreach (var tripService in tripModel.TripServices)
                {
                    var service = route.Services.FirstOrDefault(s => s.Id == tripService.ServiceId);
                    if (service != null)
                    {
                        newTrip.TripServices.Add(new Repository.EntityModels.TripService
                        {
                            Service = service,
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
            catch (Exception ex)
            {
                throw new Exception("Xảy ra lỗi khi tạo chuyến xe mới!");
                return false;
            }
        }

        public async Task<bool> UpdateTripAsync(int id, UpdateTripModel tripModel)
        {
            // Validate status
            if (!Enum.TryParse(typeof(TripStatus), tripModel.Status, true, out _))
            {
                throw new ArgumentException($"Trạng thái không hợp lệ. Trạng thái có thể là: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}.");
            }

            var existingTrip = await _tripRepository.GetTripById(id);
            if (existingTrip == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy chuyến xe có id: {id}.");
            }
            int routeId = existingTrip.RouteId == null ? default(int) : existingTrip.RouteId.Value;

            var route = await _routeRepository.GetRouteDetailByRouteIdAsync(routeId);
            if (route == null)
            {
                throw new KeyNotFoundException("Không tìm thấy tuyến xe.");
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
        public async Task<bool> UpdateTripStatusAsync(int id, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    throw new ArgumentException("Trạng thái mới không thể rỗng!");
                }
                // Validate status
                if (!Enum.TryParse(typeof(TripStatus), status, true, out _))
                {
                    throw new ArgumentException($"Trạng thái không hợp lệ. Trạng thái có thể là: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}.");
                }

                var trip = await _tripRepository.GetByIdAsync(id);

                if (trip == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy chuyến xe.");
                }
                if (!IsValidStatusTransition(status, trip.Status))
                {
                    throw new ArgumentException($"Chuyển trạng thái không hợp lệ. Trạng thái của chuyến đi không thể chuyển từ {trip.Status} sang {status}");
                }

                if (status == "DEPARTED")
                {
                    trip.ActualStartDate = DateTime.UtcNow;
                }
                // Update actual end date if the new status is "COMPLETED"
                else if (status == "COMPLETED")
                {
                    trip.ActualEndDate = DateTime.UtcNow;
                }
                trip.Status = status;

                await _tripRepository.UpdateAsync(trip);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CancelTripAsync(int id, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    throw new ArgumentException("Trạng thái mới không thể rỗng!");
                }
                // Validate status
                if (!Enum.TryParse(typeof(TripStatus), status, true, out _))
                {
                    throw new ArgumentException($"Trạng thái không hợp lệ. Trạng thái có thể là: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}.");
                }

                var trip = await _tripRepository.GetByIdAsync(id);

                if (trip == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy chuyến xe.");
                }
                if (!IsValidStatusTransition(status, trip.Status))
                {
                    throw new ArgumentException($"Chuyển trạng thái không hợp lệ. Trạng thái của chuyến đi không thể chuyển từ {trip.Status} sang {status}");
                }

                trip.Status = status;

                await _tripRepository.SoftDeleteAsync(trip);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private bool IsValidStatusTransition(string newStatus, string currentStatus)
        {
            currentStatus = currentStatus?.ToUpper();
            newStatus = newStatus?.ToUpper();

            if (!Enum.TryParse<TripStatus>(currentStatus, true, out TripStatus current))
            {
                return false;
            }

            if (!Enum.TryParse<TripStatus>(newStatus, true, out TripStatus next))
            {
                return false;
            }
            switch (current)
            {
                case TripStatus.PENDING:
                    return next == TripStatus.OPENING || next == TripStatus.CANCELED;
                case TripStatus.OPENING:
                    return next == TripStatus.DEPARTED || next == TripStatus.COMPLETED;
                case TripStatus.DEPARTED:
                    return next == TripStatus.COMPLETED;
                case TripStatus.COMPLETED:
                    // Once the trip is in "DONE" status, no further status changes are allowed
                    return false;
                default:
                    // If the current status is not recognized, disallow any status change
                    return false;
            }
        }
    }
}
