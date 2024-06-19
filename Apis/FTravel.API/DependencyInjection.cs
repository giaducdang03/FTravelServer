using FTravel.API.Middlewares;
using FTravel.Repository.Repositories.Interface;
using FTravel.Repository.Repositories;
using FTravel.Service.Mapper;
using System.Diagnostics;
using System.Security.Claims;
using FTravel.Service.Services.Interface;
using FTravel.Service.Services;

namespace FTravel.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            // use DI here

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IStationRepository, StationRepository>();
            services.AddScoped<IStationService, StationService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IRouteService, RouteService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityService, CityService>();

            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletService, WalletService>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceService, ServiceService>();

            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<IOtpService, OtpService>();

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketService, TicketService>();

            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ITicketTypeService, TicketTypeService>();

            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<ITripService, TripService>();

            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ISettingService, SettingService>();

            services.AddScoped<IBusCompanyRepository, BusCompanyRepository>();
            services.AddScoped<IBusCompanyService, BusCompanyService>();

            services.AddScoped<IClaimsService, ClaimsService>();

            services.AddScoped<IMailService, MailService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            
            services.AddHealthChecks();
            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddSingleton<PerformanceMiddleware>();
            services.AddSingleton<Stopwatch>();

            // auto mapper
            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
