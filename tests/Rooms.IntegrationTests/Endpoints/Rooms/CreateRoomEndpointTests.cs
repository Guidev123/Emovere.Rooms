using Emovere.SharedKernel.Responses;
using Microsoft.AspNetCore.Http;
using Rooms.API.Application.Commands.Rooms.Create;
using Rooms.Domain.Enums;
using Rooms.IntegrationTests.Shared;

namespace Rooms.IntegrationTests.Endpoints.Rooms
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public sealed class CreateRoomEndpointTests(IntegrationTestsFixture<Program> testsFixture)
    {
        [Fact(DisplayName = "Create Room With Success")]
        [Trait("Rooms API", "Integration Tests")]
        public async Task CreateRoomEndpoint_HandleAsync_ShouldCreateRoomWithSuccess()
        {
            // Arrange
            var command = new CreateRoomCommand(testsFixture.Faker.Name.FullName(), testsFixture.Faker.Lorem.Letter(200), DateTime.UtcNow.AddDays(1));

            // Act
            var response = await testsFixture.HttpClient.PostAsync("/api/v1/rooms", testsFixture.GetContent(command));

            // Assert
            var result = await testsFixture.GetResponse<Shared.Response<CreateRoomResponse>>(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.IsSuccess);
            Assert.Equal(StatusCode.CREATED_STATUS_CODE, (int)response.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(result.Message, EReportMessages.ROOM_CREATED_WITH_SUCCESS.GetEnumDescription());
            Assert.Empty(result.Errors ?? []);
        }
    }
}