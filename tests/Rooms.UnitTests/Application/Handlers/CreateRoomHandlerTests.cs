using Bogus;
using Emovere.Communication.IntegrationEvents;
using Emovere.Infrastructure.Bus;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;
using Moq;
using Moq.AutoMock;
using Rooms.API.Application.Commands.Rooms.Create;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;
using Rooms.Domain.Interfaces.Repositories;

namespace Rooms.UnitTests.Application.Handlers
{
    public sealed class CreateRoomHandlerTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Faker _faker = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<INotificator> _notificatorMock;
        private readonly Mock<IMessageBus> _messageBusMock;

        public CreateRoomHandlerTests()
        {
            _unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();
            _roomRepositoryMock = _mocker.GetMock<IRoomRepository>();
            _notificatorMock = _mocker.GetMock<INotificator>();
            _messageBusMock = _mocker.GetMock<IMessageBus>();
        }

        [Fact(DisplayName = "Should Create Room With Success")]
        [Trait("Application", "Room Handlers")]
        public async Task ExecuteAsync_CreateValidRoom_ShouldCreateWithSuccess()
        {
            // Arrange
            var command = new CreateRoomCommand(_faker.Address.FullAddress(), _faker.Lorem.Paragraph(), DateTime.UtcNow.AddDays(1));
            command.SetHostId(Guid.NewGuid());

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(true);

            _roomRepositoryMock
                .Setup(r => r.Create(It.IsAny<Room>()))
                .Verifiable();

            _messageBusMock
                .Setup(x => x.PublishAsync(It.IsAny<CreatedRoomIntegrationEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var handler = new CreateRoomHandler(_notificatorMock.Object, _messageBusMock.Object, _unitOfWorkMock.Object, _roomRepositoryMock.Object);

            // Act
            var result = await handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Errors);
            Assert.Equal(StatusCode.CREATED_STATUS_CODE, result.Code);
            Assert.Equal(result.Message, EReportMessages.ROOM_CREATED_WITH_SUCCESS.GetEnumDescription());
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _roomRepositoryMock.Verify(r => r.Create(It.IsAny<Room>()), Times.Once);
            _messageBusMock.Verify(x => x.PublishAsync(It.IsAny<CreatedRoomIntegrationEvent>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Should Fail to Create Room With Invalid Data")]
        [Trait("Application", "Room Handlers")]
        public async Task ExecuteAsync_TryCreateRoomWithInvalidData_ShouldReturnValidationErrors()
        {
            // Arrange
            SetupNotifications();
            var command = new CreateRoomCommand(string.Empty, string.Empty, DateTime.UtcNow);
            command.SetHostId(Guid.NewGuid());

            _messageBusMock
                .Setup(x => x.PublishAsync(It.IsAny<CreatedRoomIntegrationEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var handler = new CreateRoomHandler(_notificatorMock.Object, _messageBusMock.Object, _unitOfWorkMock.Object, _roomRepositoryMock.Object);

            // Act
            var result = await handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.Equal(5, result.Errors.Count);
            Assert.Equal(StatusCode.BAD_REQUEST_STATUS_CODE, result.Code);
            Assert.Equal(result.Message, EReportMessages.VALIDATION_ERROR.GetEnumDescription());
            _messageBusMock.Verify(x => x.PublishAsync(It.IsAny<CreatedRoomIntegrationEvent>(), CancellationToken.None), Times.Never);
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