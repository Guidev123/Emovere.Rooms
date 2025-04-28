using Emovere.SharedKernel.DomainObjects;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;

namespace Rooms.UnitTests.Domain.Entities
{
    public sealed class RoomTests
    {
        [Fact(DisplayName = "Room Should Throw Domain Exception if It's Invalid")]
        [Trait("Domain", "Room Entity Tests")]
        public void Room_Validate_ShouldThrowDomainExceptionIfItsInvalid()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                var room = new Room(Guid.Empty, "Test", "Test", ERoomType.Open, DateTime.UtcNow.AddHours(1));
            });

            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                var room = new Room(Guid.NewGuid(), string.Empty, "Test", ERoomType.Open, DateTime.UtcNow.AddHours(1));
            });

            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                var room = new Room(Guid.NewGuid(), "Test", string.Empty, ERoomType.Open, DateTime.UtcNow.AddHours(1));
            });

            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                var room = new Room(Guid.NewGuid(), "Test", "Test", ERoomType.Open, DateTime.UtcNow);
            });
        }

        [Fact(DisplayName = "Room Should Close With Success")]
        [Trait("Domain", "Room Entity Tests")]
        public void Room_CloseRoom_ShouldUpdateRoomStatusToClose()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", ERoomType.InvitationOnly, DateTime.UtcNow.AddHours(1));

            // Act
            room.CloseRoom();

            // Assert
            Assert.Equal(ERoomStatus.Closed, room.Status);
        }

        [Fact(DisplayName = "Room Should Throw Domain Exception to Close If Room is Already Closed")]
        [Trait("Domain", "Room Entity Tests")]
        public void Room_CloseRoom_SouldThrowDomainExceptionIf()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", ERoomType.InvitationOnly, DateTime.UtcNow.AddHours(1));
            room.CloseRoom();

            // Act & Assert
            Assert.Throws<DomainException>(room.CloseRoom);
        }

        [Fact(DisplayName = "Room Should Update Plan and Max Participant Quantity")]
        [Trait("Domain", "Room Entity Tests")]
        public void Room_CloseRoom_ShouldUpdatePlanWithSuccess()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", ERoomType.InvitationOnly, DateTime.UtcNow.AddHours(1));

            // Act
            room.SetPlan(ERoomPlan.Premium);

            // Assert
            Assert.Equal(ERoomPlan.Premium, room.Plan);
            Assert.Equal(Room.MAX_PREMIUM_PARTICIPANTS, room.RoomSpecification.MaxParticipantsNumber);
        }
    }
}
