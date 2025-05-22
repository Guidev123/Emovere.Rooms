using Bogus;
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
        private readonly Faker _faker = new();
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

        [Fact(DisplayName = "Should Add Participant to Standard Room With Success")]
        [Trait("Domain", "Room Service")]
        public async Task RoomService_AddParticipant_ShouldAddParticipantToStandardRoomWithSuccess()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", DateTime.UtcNow.AddHours(1));
            var participant = GenerateValidParticipant(room.Id);

            var roomService = GetRoomService(SetupStrategy(new StandardRoomAddParticipantStrategy(), room, participant));

            // Act
            var resultIsSuccess = await roomService.AddParticipantAsync(participant, room);

            // Assert
            Assert.True(resultIsSuccess);
            Assert.Equal(1, room.ParticipantsQuantity);
            _roomRepository.Verify(m => m.Update(room), Times.Once());
            _strategyFactory.Verify(m => m.GetStrategy(room.Plan), Times.Once());
        }

        [Fact(DisplayName = "Should Fail To Add Participant to Standard Room When Room is full")]
        [Trait("Domain", "Room Service")]
        public async Task RoomService_AddParticipant_ShouldFailToAddParticipantToStandardRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var room = new Room(roomId, "Test", "Test", DateTime.UtcNow.AddHours(1));
            var participants = GenerateValidParticipant(roomId).Generate(Room.MAX_STANDARD_PARTICIPANTS + 1);
            var strategy = new StandardRoomAddParticipantStrategy();

            _strategyFactory.Setup(f => f.GetStrategy(room.Plan)).Returns(strategy);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

            var roomService = GetRoomService(strategy);

            // Act
            bool lastResult = false;
            int successfulAdditions = 0;

            foreach (var participant in participants)
            {
                _participantStrategyMock.Setup(s => s.AddParticipant(participant, room))
                    .Returns(room.ParticipantsQuantity < Room.MAX_STANDARD_PARTICIPANTS);

                var result = await roomService.AddParticipantAsync(participant, room);
                if (result) successfulAdditions++;
                lastResult = result;
            }

            // Assert
            Assert.False(lastResult);
            Assert.Equal(Room.MAX_STANDARD_PARTICIPANTS, room.ParticipantsQuantity);
            Assert.Equal(Room.MAX_STANDARD_PARTICIPANTS, successfulAdditions);
            _roomRepository.Verify(m => m.Update(room), Times.Exactly(Room.MAX_STANDARD_PARTICIPANTS));
        }

        [Fact(DisplayName = "Should Fail To Add Participant to Premium Room When Room is full")]
        [Trait("Domain", "Room Service")]
        public async Task RoomService_AddParticipant_ShouldFailToAddParticipantToPremiumRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var room = new Room(roomId, "Test", "Test", DateTime.UtcNow.AddHours(1));
            room.SetPlan(ERoomPlan.Premium);

            var participants = GenerateValidParticipant(roomId).Generate(Room.MAX_PREMIUM_PARTICIPANTS + 1);
            var strategy = new PremiumRoomAddParticipantStrategy();

            _strategyFactory.Setup(f => f.GetStrategy(room.Plan)).Returns(strategy);

            var roomService = GetRoomService(strategy);

            // Act
            bool lastResult = false;
            int successfulAdditions = 0;

            foreach (var participant in participants)
            {
                _participantStrategyMock.Setup(s => s.AddParticipant(participant, room))
                    .Returns(room.ParticipantsQuantity < Room.MAX_PREMIUM_PARTICIPANTS);

                var result = await roomService.AddParticipantAsync(participant, room);
                if (result) successfulAdditions++;
                lastResult = result;
            }

            // Assert
            Assert.False(lastResult);
            Assert.Equal(Room.MAX_PREMIUM_PARTICIPANTS, room.ParticipantsQuantity);
            Assert.Equal(Room.MAX_PREMIUM_PARTICIPANTS, successfulAdditions);
            _roomRepository.Verify(m => m.Update(room), Times.Exactly(Room.MAX_PREMIUM_PARTICIPANTS));
        }

        [Fact(DisplayName = "Should Fail To Add Participant to Vip Room When Room is full")]
        [Trait("Domain", "Room Service")]
        public async Task RoomService_AddParticipant_ShouldFailToAddParticipantToVipRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var room = new Room(roomId, "Test", "Test", DateTime.UtcNow.AddHours(1));
            room.SetPlan(ERoomPlan.Vip);

            var participants = GenerateValidParticipant(roomId).Generate(Room.MAX_VIP_PARTICIPANTS + 1);
            var strategy = new VipRoomAddParticipantStrategy();

            _strategyFactory.Setup(f => f.GetStrategy(room.Plan)).Returns(strategy);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

            var roomService = GetRoomService(strategy);

            // Act
            bool lastResult = false;
            int successfulAdditions = 0;

            foreach (var participant in participants)
            {
                _participantStrategyMock.Setup(s => s.AddParticipant(participant, room))
                    .Returns(room.ParticipantsQuantity < Room.MAX_VIP_PARTICIPANTS);

                var result = await roomService.AddParticipantAsync(participant, room);
                if (result) successfulAdditions++;
                lastResult = result;
            }

            // Assert
            Assert.False(lastResult);
            Assert.Equal(Room.MAX_VIP_PARTICIPANTS, room.ParticipantsQuantity);
            Assert.Equal(Room.MAX_VIP_PARTICIPANTS, successfulAdditions);
            _roomRepository.Verify(m => m.Update(room), Times.Exactly(Room.MAX_VIP_PARTICIPANTS));
        }

        [Fact(DisplayName = "Should Fail To Add Participant to Exclusive Room When Room is full")]
        [Trait("Domain", "Room Service")]
        public async Task RoomService_AddParticipant_ShouldFailToAddParticipantToExclusiveRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var room = new Room(roomId, "Test", "Test", DateTime.UtcNow.AddHours(1));
            room.SetPlan(ERoomPlan.Exclusive);

            var participants = GenerateValidParticipant(roomId).Generate(Room.MAX_EXCLUSIVE_PARTICIPANTS + 1);
            var strategy = new ExclusiveRoomAddParticipantStrategy();

            _strategyFactory.Setup(f => f.GetStrategy(room.Plan)).Returns(strategy);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

            var roomService = GetRoomService(strategy);

            // Act
            bool lastResult = false;
            int successfulAdditions = 0;

            foreach (var participant in participants)
            {
                _participantStrategyMock.Setup(s => s.AddParticipant(participant, room))
                    .Returns(room.ParticipantsQuantity < Room.MAX_EXCLUSIVE_PARTICIPANTS);

                var result = await roomService.AddParticipantAsync(participant, room);
                if (result) successfulAdditions++;
                lastResult = result;
            }

            // Assert
            Assert.False(lastResult);
            Assert.Equal(Room.MAX_EXCLUSIVE_PARTICIPANTS, room.ParticipantsQuantity);
            Assert.Equal(Room.MAX_EXCLUSIVE_PARTICIPANTS, successfulAdditions);
            _roomRepository.Verify(m => m.Update(room), Times.Exactly(Room.MAX_EXCLUSIVE_PARTICIPANTS));
        }

        #region Private Methods

        private Faker<Participant> GenerateValidParticipant(Guid roomId)
        {
            return new Faker<Participant>().CustomInstantiator(faker =>
                new Participant(Guid.NewGuid(), roomId, _faker.Internet.Email()));
        }

        private RoomCapacityValidationService GetRoomService(IAddParticipantStrategy strategy)
        {
            return new(
                _strategyFactory.Object,
                _notificator.Object,
                _roomRepository.Object,
                _unitOfWorkMock.Object
            );
        }

        private IAddParticipantStrategy SetupStrategy(IAddParticipantStrategy strategy, Room room, Participant participant)
        {
            _participantStrategyMock
                .Setup(s => s.AddParticipant(participant, room))
                .Returns(true);

            _strategyFactory
                .Setup(f => f.GetStrategy(room.Plan))
                .Returns(strategy);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(true);

            return strategy;
        }

        #endregion Private Methods
    }
}