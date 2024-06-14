using AutoMapper;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            // add mapper model
            CreateMap<CreateRouteModel, Route>().ReverseMap();

            CreateMap<UserModel, User>().ReverseMap().ForMember(dest => dest.Role, opt => opt.Ignore());
            CreateMap<Customer, User>().ReverseMap();
            CreateMap<Wallet, WalletModel>();
            CreateMap<Transaction, TransactionModel>();
            CreateMap<Route, RouteModel>()
                .ForMember(dest => dest.StartPoint, opt => opt.MapFrom(src => src.StartPointNavigation.Name))
                .ForMember(dest => dest.EndPoint, opt => opt.MapFrom(src => src.EndPointNavigation.Name))
                .ForMember(dest => dest.BusCompanyName, opt => opt.MapFrom(src => src.BusCompany.Name));
            CreateMap<StationModel, Station>().ReverseMap();

            CreateMap<City, CityModel>().ReverseMap();
            CreateMap<TicketType, TicketTypeModel>();
            CreateMap<AccountModel, User>().ReverseMap();

            CreateMap<Repository.EntityModels.Service, ServiceModel>()
            .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route != null ? src.Route.Name : string.Empty))
            .ForMember(dest => dest.StationName, opt => opt.MapFrom(src => src.Station != null ? src.Station.Name : string.Empty))
            .ReverseMap();
            
            CreateMap<CreateServiceModel, Repository.EntityModels.Service>()
            .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)));
            
            CreateMap<UpdateServiceModel, Repository.EntityModels.Service>()
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)));

            CreateMap<Trip, TripModel>()
            .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route != null ? src.Route.Name : string.Empty));
            CreateMap<Trip, TripModel>()
            .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route.Name))
            .ForMember(dest => dest.Tickets, opt => opt.Ignore()); 

            CreateMap<Ticket, TicketModel>()
            .ForMember(dest => dest.TicketTypeName, opt => opt.MapFrom(src => src.TicketType.Name));

            CreateMap<CreateTripModel, Trip>()
            .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)))
            .ForMember(dest => dest.TripServices, opt => opt.Ignore())
            .ForMember(dest => dest.TripTicketTypes, opt => opt.Ignore());

            CreateMap<UpdateTripModel, Trip>()
            .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)))
            .ForMember(dest => dest.TripServices, opt => opt.Ignore())
            .ForMember(dest => dest.TripTicketTypes, opt => opt.Ignore());
            CreateMap<CreateBusCompanyModel, BusCompany>()
            .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)));

            CreateMap<CreateAccountModel, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<OrderModel, Order>();
        }
    }
}
