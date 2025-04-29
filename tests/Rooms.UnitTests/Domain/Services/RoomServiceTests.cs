using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Notifications;
using Moq;
using Moq.AutoMock;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Domain.Services;
using Rooms.Domain.Strategies;
using Rooms.Domain.Strategies.Factories;

namespace Rooms.UnitTests.Domain.Services
{
    public sealed class RoomServiceTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IRoomRepository> _roomRepository;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAddParticipantStrategyFactory> _strategyFactory;
        private readonly Mock<INotificator> _notificator;
        private readonly Mock<IAddParticipantStrategy> _participantStrategyMock;

        public RoomServiceTests()
        {
            _roomRepository = _mocker.GetMock<IRoomRepository>();
            _unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();   
            _strategyFactory = _mocker.GetMock<IAddParticipantStrategyFactory>();
            _notificator = _mocker.GetMock<INotificator>();
            _participantStrategyMock = _mocker.GetMock<IAddParticipantStrategy>();
        }

        [Fact(DisplayName = "Should Add Participant to Room")]
        [Trait("Domain", "Room Service")]
        public async Task RoomService_AddParticipant_ShouldAddParticipantToRoom()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", ERoomType.Open, DateTime.UtcNow.AddHours(1));
            var participant = new Participant(Guid.NewGuid(), room.Id, "email@test.com");

            _participantStrategyMock
                .Setup(s => s.AddParticipant(participant, room))
                .Returns(true);

            _strategyFactory
                .Setup(f => f.GetStrategy(room.Plan))
                .Returns(_participantStrategyMock.Object);

            _unitOfWorkMock
                .Setup(u => u.CommitAsync())
                .ReturnsAsync(true);

            var roomService = new RoomService(
                _strategyFactory.Object,
                _notificator.Object,
                _roomRepository.Object,
                _unitOfWorkMock.Object
            );

            // Act
            var resultIsSuccess = await roomService.AddParticipantAsync(participant, room);

            // Assert
            Assert.True(resultIsSuccess);
            _unitOfWorkMock.Verify(m => m.CommitAsync(), Times.Once());
            _roomRepository.Verify(m => m.Update(room), Times.Once());
            _strategyFactory.Verify(m => m.GetStrategy(room.Plan), Times.Once());
            _participantStrategyMock.Verify(m => m.AddParticipant(participant, room), Times.Once());
        }

    }
}
