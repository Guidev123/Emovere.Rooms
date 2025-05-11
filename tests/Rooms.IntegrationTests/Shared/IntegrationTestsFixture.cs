using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
        private readonly HttpClient _authHttpClient;

        public Faker Faker = new();

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:7103"),
            };

            Factory = new SalesAppFactory<TProgram>();
            HttpClient = Factory.CreateClient(clientOptions);
            _authHttpClient = new()
            {
                BaseAddress = new Uri("https://localhost:7275")
            };
        }

        public async Task LoginUserAsync()
        {
            var response = await _authHttpClient.PostAsJsonAsync("/api/v1/authentication/login", new LoginRequest("test@test.com", "Test@123"));

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<Response<Token>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token?.Data?.AccessToken);
        }

        public void Dispose()
        {
            Factory.Dispose();
            HttpClient.Dispose();
            _authHttpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public record Response(bool? IsSuccess, List<string?>? Errors, string? Message);
    public record LoginRequest(string Email, string Password);
    public record Token(string AccessToken);
    public record Response<T>(bool? IsSuccess, List<string?>? Errors, string? Message, T? Data);
}