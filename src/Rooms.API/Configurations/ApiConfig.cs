using Emovere.Infrastructure.Bus;
using Emovere.Infrastructure.Email;
using Emovere.Infrastructure.EventSourcing;
using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MidR.DependencyInjection;
using Rooms.API.Endpoints;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Domain.Interfaces.Services;
using Rooms.Domain.Services;
using Rooms.Domain.Strategies.Factories;
using Rooms.Infrastructure.BackgroundServices;
using Rooms.Infrastructure.Data.Contexts;
using Rooms.Infrastructure.Data.Repositories;
using SendGrid.Extensions.DependencyInjection;
using System.Reflection;

namespace Rooms.API.Configurations
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApi();
            builder.Services.AddEventStoreConfiguration();
            builder.Services.AddHttpContextAccessor();

            return builder; 
        }

        public static WebApplicationBuilder AddSecurityConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            return builder;
        }

        public static WebApplicationBuilder AddRoomStrategyConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAddParticipantStrategyFactory, AddParticipantStrategyFactory>();

            return builder;
        }

        public static WebApplicationBuilder AddEmailServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSendGrid(x =>
            {
                x.ApiKey = builder.Configuration.GetValue<string>("EmailSettings:ApiKey");
            });
            builder.Services.AddScoped<IEmailService, EmailService>();

            return builder;
        }

        public static WebApplicationBuilder AddDomainServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRoomService, RoomService>();

            return builder;
        }

        public static WebApplicationBuilder AddNotificationConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<INotificator, Notificator>();
         
            return builder;
        }

        public static WebApplicationBuilder AddMediatorHandlersConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
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

        public static WebApplicationBuilder AddMessageBusConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMessageBus(builder.Configuration.GetConnectionString("MessageBusConnection") ?? string.Empty);

            return builder;
        }

        public static WebApplicationBuilder AddRepositoriesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            return builder;
        }

        public static WebApplicationBuilder AddSwaggerConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "API SalesSystem",
                    Contact = new OpenApiContact() { Name = "Guilherme Nascimento", Email = "guirafaelrn@gmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/license/MIT") }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter the JWT token in this format: Bearer <your token>",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return builder;
        }

        public static WebApplication UseSwaggerConfig(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));

            return app;
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
