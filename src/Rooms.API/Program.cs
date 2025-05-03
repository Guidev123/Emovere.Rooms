using Rooms.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddInfrastructureServicesConfiguration()
    .AddServicesConfiguration()
    .AddApplicationServicesConfiguration()
    .AddDomainServicesConfiguration()
       .AddRoomStrategyConfiguration()
       .AddDomainServicesConfiguration()
       .AddNotificationConfiguration()
       .AddMediatorHandlersConfiguration()
       .AddContextsConfiguration()
       .AddRepositoriesConfiguration()
       .AddMessageBusConfiguration()
       .AddEmailServicesConfiguration()
       .AddSwaggerConfig()
       .AddSecurityConfig()
       .AddBackgroundServicesConfiguration()
       .AddCustomMiddlewares();

var app = builder.Build();

app.UseEndpoints()
    .UseMiddlewares()
    .UseOpenApi()
    .UseApiSecurityConfig()
    .UseSwaggerConfig(builder);

app.Run();

public partial class Program
{ }