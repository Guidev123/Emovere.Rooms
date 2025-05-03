using Rooms.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder
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
       .AddSecurityConfiguration()
       .AddBackgroundServicesConfiguration()
       .AddServicesConfiguration()
       .AddCustomMiddlewares();

var app = builder.Build();

app.UseEndpoints()
    .UseMiddlewares()
    .UseOpenApi()
    .UseSwaggerConfig()
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

app.Run();

public partial class Program
{ }