using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
namespace Rooms.IntegrationTests.Shared
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

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

        public StringContent GetContent<T>(T command)
            => new(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json");

        public async Task<T> GetResponse<T>(HttpResponseMessage response)
            => JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        public void Dispose()
        {
            Factory.Dispose();
            HttpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
    public record Response(bool? IsSuccess, List<string?>? Errors, string? Message);
    public record Response<T>(bool? IsSuccess, List<string?>? Errors, string? Message, T? Data);
}
