﻿using FTravel.API.Middlewares;
using FTravel.Repository.Repositories.Interface;
using FTravel.Repository.Repositories;
using FTravel.Service.Mapper;
using System.Diagnostics;
using System.Security.Claims;

namespace FTravel.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            // use DI here


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
