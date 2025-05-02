using Rooms.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddDomainServicesConfiguration()
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
       .AddServicesConfiguration();

var app = builder.Build();

app.UseEndpoints()
    .UseOpenApi()
    .UseSwaggerConfig()
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

app.Run();

public partial class Program
{ }