﻿using FTravel.API.Middlewares;
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

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICityRepository, CityRepository>();

            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletService, WalletService>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionService, TransactionService>();



            services.AddScoped<IClaimsService, ClaimsService>();
            
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
