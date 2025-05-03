using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rooms.API.Configurations.Extensions;

namespace Rooms.API.Configurations
{
    internal static class SecurityConfig
    {
        public static WebApplicationBuilder AddSecurityConfig(this WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection(nameof(KeycloakExtension));
            if (!appSettingsSection.Exists())
                throw new InvalidOperationException();

            var appSettings = appSettingsSection.Get<KeycloakExtension>();
            if (string.IsNullOrEmpty(appSettings?.AuthorizationUrl))
                throw new InvalidOperationException();

            builder.Services.Configure<KeycloakExtension>(appSettingsSection);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.Authority = appSettings.Issuer;
                o.Audience = appSettings.Audience;
                o.MetadataAddress = appSettings.MetadataAddress;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidAudience = appSettings.Audience,
                    ValidIssuer = appSettings.Issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });
            builder.Services.AddAuthorization();

            return builder;
        }

        public static WebApplication UseApiSecurityConfig(this WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}