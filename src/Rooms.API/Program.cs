using Rooms.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddInfrastructureServicesConfiguration()
    .AddServicesConfiguration()
    .AddApplicationServicesConfiguration()
    .AddDomainServicesConfiguration()
    .AddSerilog()
       .AddRoomStrategyConfiguration()
       .AddDomainServicesConfiguration()
       .AddNotificationConfiguration()
       .AddMediatorHandlersConfiguration()
       .AddContextsConfiguration()
       .AddRepositoriesConfiguration()
       .AddMessageBusConfiguration()
       .AddEmailServicesConfiguration()
       .AddSwaggerConfig()
       .AddSecurityConfiguration()
       .AddBackgroundServicesConfiguration()
       .AddCustomMiddlewares();

var app = builder.Build();

app.UseEndpoints()
    .UseSerilogSettings()
    .UseMiddlewares()
    .UseOpenApi()
    .UseApiSecurityConfig()
    .UseSwaggerConfig();

app.Run();

public partial class Program
{ }