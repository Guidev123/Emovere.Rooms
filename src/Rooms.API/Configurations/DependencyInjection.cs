using Emovere.SharedKernel.Notifications;
using Microsoft.EntityFrameworkCore;
using MidR.DependencyInjection;
using Rooms.Domain.Interfaces.Services;
using Rooms.Domain.Services;
using Rooms.Domain.Strategies.Contexts;
using Rooms.Domain.Strategies.Factories;
using Rooms.Infrastructure.Data.Contexts;
using System.Reflection;

namespace Rooms.API.Configurations
{
    public static class DependencyInjection
    {
        public static void AddDependenciesConfiguration(this WebApplicationBuilder builder)
        {
            builder.AddDomainServices();
            builder.AddRoomStrategy();
            builder.AddNotification();
            builder.AddHandlers();
            builder.AddContextsConfiguration();
        }

        private static void AddRoomStrategy(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAddParticipantStrategyFactory, AddParticipantStrategyFactory>();
            builder.Services.AddScoped<IAddParticipantContext, AddParticipantContext>();
        }

        private static void AddDomainServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRoomService, RoomService>();
        }

        private static void AddNotification(this WebApplicationBuilder builder)
            => builder.Services.AddScoped<INotificator, Notificator>();

        private static void AddHandlers(this WebApplicationBuilder builder)
            => builder.Services.AddMidR(Assembly.GetExecutingAssembly());

        private static void AddContextsConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<RoomsWriteDbContext>(x =>
            x.UseSqlServer(builder.Configuration.GetConnectionString("WriteModelConnection")));

            builder.Services.AddDbContext<RoomsReadDbContext>(x => 
                x.UseMongoDB(builder.Configuration.GetConnectionString("ReadModelConnection") ?? string.Empty,
                builder.Configuration.GetConnectionString("ReadModelDatabase") ?? string.Empty));
        }
    }
}
