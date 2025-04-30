using Emovere.Infrastructure.Email;
using Emovere.Infrastructure.EventSourcing;
using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Notifications;
using Microsoft.EntityFrameworkCore;
using MidR.DependencyInjection;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Domain.Interfaces.Services;
using Rooms.Domain.Services;
using Rooms.Domain.Strategies.Factories;
using Rooms.Infrastructure.Data.Contexts;
using Rooms.Infrastructure.Data.Repositories;
using SendGrid.Extensions.DependencyInjection;

namespace Rooms.API.Configurations
{
    public static class DependencyInjection
    {
        private const string HANDLERS_ASSEMBLY_NAME = "Rooms.Application";

        public static void AddDependenciesConfiguration(this WebApplicationBuilder builder)
        {
            builder.AddDomainServices();
            builder.AddRoomStrategy();
            builder.AddNotification();
            builder.AddMediatorHandlers();
            builder.AddContextsConfiguration();
            builder.AddRepositories();
            builder.AddEmailServices();
            builder.Services.AddEventStoreConfiguration();
            builder.Services.AddHttpContextAccessor();
        }

        private static void AddRoomStrategy(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAddParticipantStrategyFactory, AddParticipantStrategyFactory>();
        }

        private static void AddEmailServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSendGrid(x =>
            {
                x.ApiKey = builder.Configuration.GetValue<string>("EmailSettings:ApiKey");
            });
            builder.Services.AddScoped<IEmailService, EmailService>();
        }

        private static void AddDomainServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRoomService, RoomService>();
        }

        private static void AddNotification(this WebApplicationBuilder builder)
            => builder.Services.AddScoped<INotificator, Notificator>();

        private static void AddMediatorHandlers(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
            builder.Services.AddMidR(HANDLERS_ASSEMBLY_NAME);
        }

        private static void AddContextsConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<RoomsWriteDbContext>(x =>
            x.UseSqlServer(builder.Configuration.GetConnectionString("WriteModelConnection")));

            builder.Services.AddDbContext<RoomsReadDbContext>(x => 
                x.UseMongoDB(builder.Configuration.GetConnectionString("ReadModelConnection") ?? string.Empty,
                builder.Configuration.GetConnectionString("ReadModelDatabase") ?? string.Empty));
        }

        private static void AddRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
