using Bogus;
using Emovere.SharedKernel.Notifications;
using Moq.AutoMock;
using Moq;
using Rooms.Domain.Interfaces.Repositories;
using Emovere.Infrastructure.Bus;
using Xunit;
using Rooms.Domain.Entities;
using Rooms.Domain.Interfaces.Services;
using Emovere.SharedKernel.Responses;
using Rooms.API.Application.Commands.Rooms.AddParticipant;

namespace Rooms.UnitTests.Application.Handlers
{
    public sealed class AddParticipantToRoomHandlerTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Faker _faker = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<INotificator> _notificatorMock;
        private readonly Mock<IMessageBus> _messageBusMock;
        private readonly Mock<IRoomCapacityValidationService> _roomCapacityValidationServiceMock;

        public AddParticipantToRoomHandlerTests()
        {
            _unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();
            _roomRepositoryMock = _mocker.GetMock<IRoomRepository>();
            _notificatorMock = _mocker.GetMock<INotificator>();
            _messageBusMock = _mocker.GetMock<IMessageBus>();
            _roomCapacityValidationServiceMock = _mocker.GetMock<IRoomCapacityValidationService>();
        }

        [Fact(DisplayName = "Should Add Participant To Room With Success")]
        [Trait("Application", "Room Handlers")]
        public async Task ExecuteAsync_AddValidParticipant_ShouldAddWithSuccess()
        {
            // Arrange
            var hostId = Guid.NewGuid();
            var room = new Room(hostId, _faker.Lorem.Text(), _faker.Lorem.Paragraph(), DateTime.UtcNow.AddDays(3));
            var command = new AddParticipantCommand(room.Id, Guid.NewGuid(), "participant@test.com.br");

            _roomCapacityValidationServiceMock
                .Setup(r => r.AddParticipantAsync(It.IsAny<Participant>(), It.IsAny<Room>()))
                .ReturnsAsync(true);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(true);

            _roomRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.RoomId))
                .ReturnsAsync(room);

            var handler = new AddParticipantHandler(_notificatorMock.Object,
                                                    _roomRepositoryMock.Object,
                                                    _unitOfWorkMock.Object,
                                                    _roomCapacityValidationServiceMock.Object);

            // Act
            var result = await handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Errors);
            Assert.Equal(StatusCode.NO_CONTENT_STATUS_CODE, result.Code);
        }

        #region Private Methods

        private void SetupNotifications()
        {
            var notifications = new List<Notification>();
            _notificatorMock.Setup(n => n.HandleNotification(It.IsAny<Notification>()))
                .Callback<Notification>(notifications.Add);

            _notificatorMock.Setup(n => n.GetNotifications())
                .Returns(() => notifications);
        }

        #endregion Private Methods
    }
}