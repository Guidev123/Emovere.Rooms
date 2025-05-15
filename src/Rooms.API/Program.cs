using Emovere.WebApi.Config;
using Rooms.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddSharedConfig()
        .AddServicesConfiguration()
        .AddApplicationServicesConfiguration()
        .AddDomainServicesConfiguration()
        .AddRoomStrategyConfiguration()
        .AddDomainServicesConfiguration()
        .AddMediatorHandlersConfiguration()
        .AddContextsConfiguration()
        .AddRepositoriesConfiguration()
        .AddSwaggerConfig()
        .AddBackgroundServicesConfiguration();

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