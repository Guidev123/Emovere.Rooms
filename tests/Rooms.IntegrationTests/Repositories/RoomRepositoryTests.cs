using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rooms.Domain.Entities;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Infrastructure.Data.Contexts;
using Rooms.IntegrationTests.Shared;

namespace Rooms.IntegrationTests.Repositories
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class RoomRepositoryTests : IClassFixture<SalesAppFactory<Program>>, IDisposable
    {
        private readonly RoomsWriteDbContext _writeDbContext;
        private readonly RoomsReadDbContext _readDbContext;
        private readonly IRoomRepository _roomRepository;
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public RoomRepositoryTests(SalesAppFactory<Program> factory, IntegrationTestsFixture<Program> testsFixture)
        {
            var scope = factory.Services.CreateScope();
            _writeDbContext = scope.ServiceProvider.GetRequiredService<RoomsWriteDbContext>();
            _readDbContext = scope.ServiceProvider.GetRequiredService<RoomsReadDbContext>();
            _roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Get Room By Id With Success")]
        [Trait("Rooms Repository", "Integration Tests")]
        public async Task RoomRepository_GetByIdAsync_ShouldReturnRoomWhenExists()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4));
            _readDbContext.Rooms.Add(room);
            _readDbContext.SaveChanges();

            // Act
            var result = await _roomRepository.GetByIdAsync(room.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(room, result);
        }

        [Fact(DisplayName = "Get All Rooms By Host Id")]
        [Trait("Rooms Repository", "Integration Tests")]
        public async Task RoomRepository_GetAllByHostIdAsync_ShouldReturnPagedRoomsList()
        {
            // Arrange
            var hostId = Guid.NewGuid();

            var rooms = new List<Room>()
            {
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
                new(hostId, _testsFixture.Faker.Address.FullAddress(), _testsFixture.Faker.Lorem.Text(), DateTime.UtcNow.AddDays(4)),
            };

            _readDbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
            _readDbContext.Rooms.AddRange(rooms);
            _readDbContext.SaveChanges();

            // Act
            var result = await _roomRepository.GetAllByHostIdAsync(hostId, 1, 5);

            // Assert
            Assert.NotNull(result.Rooms);
            Assert.Equal(5, result.Rooms.Count());
        }

        public void Dispose()
        {
            _readDbContext.Dispose();
            _writeDbContext.Dispose();
            _roomRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}