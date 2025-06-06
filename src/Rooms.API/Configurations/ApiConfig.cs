﻿using Microsoft.EntityFrameworkCore;
using MidR.DependencyInjection;
using Rooms.API.Application.Services;
using Rooms.API.Application.Services.Interfaces;
using Rooms.API.Endpoints;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Domain.Interfaces.Services;
using Rooms.Domain.Services;
using Rooms.Domain.Strategies.Factories;
using Rooms.Infrastructure.BackgroundServices;
using Rooms.Infrastructure.Data.Contexts;
using Rooms.Infrastructure.Data.Repositories;
using System.Reflection;

namespace Rooms.API.Configurations
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApi();

            return builder;
        }

        public static WebApplicationBuilder AddRoomStrategyConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAddParticipantStrategyFactory, AddParticipantStrategyFactory>();

            return builder;
        }

        public static WebApplicationBuilder AddDomainServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IRoomCapacityValidationService, RoomCapacityValidationService>();

            return builder;
        }

        public static WebApplicationBuilder AddApplicationServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IRoomService, RoomService>();

            return builder;
        }

        public static WebApplicationBuilder AddMediatorHandlersConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMidR(Assembly.GetExecutingAssembly());

            return builder;
        }

        public static WebApplicationBuilder AddBackgroundServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<RoomProjectionService>();

            return builder;
        }

        public static WebApplicationBuilder AddContextsConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<RoomsWriteDbContext>(x =>
            x.UseSqlServer(builder.Configuration.GetConnectionString("WriteModelConnection")));

            builder.Services.AddDbContext<RoomsReadDbContext>(x =>
                x.UseMongoDB(builder.Configuration.GetConnectionString("ReadModelConnection") ?? string.Empty,
                builder.Configuration.GetConnectionString("ReadModelDatabase") ?? string.Empty));

            return builder;
        }

        public static WebApplicationBuilder AddRepositoriesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            return builder;
        }

        public static WebApplication UseEndpoints(this WebApplication app)
        {
            app.MapEndpoints();

            return app;
        }

        public static WebApplication UseOpenApi(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            return app;
        }
    }
}