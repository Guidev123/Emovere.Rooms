using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Rooms.API.Configurations.Extensions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Rooms.IntegrationTests.Shared
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>>
    { }

    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public readonly SalesAppFactory<TProgram> Factory;
        public HttpClient HttpClient;
        public Faker Faker = new();

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost"),
            };

            Factory = new SalesAppFactory<TProgram>();
            HttpClient = Factory.CreateClient(clientOptions);
        }

        public StringContent GetContent(object data, NamingStrategy namingStrategy)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = namingStrategy
                }
            };

            var json = JsonConvert.SerializeObject(data, settings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<T?> DeserializeObjectResponseAsync<T>(HttpResponseMessage response, NamingStrategy namingStrategy)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = namingStrategy
                }
            };

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public async Task LoginUserAsync(HttpClient httpClient)
        {
            using var scope = Factory.Services.CreateScope();
            var keycloak = scope.ServiceProvider.GetRequiredService<IOptions<KeycloakExtension>>().Value;

            var requestContent = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", keycloak.ClientId),
                new KeyValuePair<string, string>("client_secret", keycloak.ClientSecret)
            ]);

            var newHttpClient = new HttpClient();

            var response = await newHttpClient.PostAsync(
                $"{keycloak.BaseUrl}/realms/{keycloak.CurrentRealm}/protocol/openid-connect/token",
                requestContent).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var token = await DeserializeObjectResponseAsync<Token>(response, new SnakeCaseNamingStrategy());

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }

        public void Dispose()
        {
            Factory.Dispose();
            HttpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public record Response(bool? IsSuccess, List<string?>? Errors, string? Message);
    public record Token(string AccessToken);
    public record Response<T>(bool? IsSuccess, List<string?>? Errors, string? Message, T? Data);
}