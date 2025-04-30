using Emovere.SharedKernel.DomainObjects;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;
using Xunit;

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
                var room = new Room(Guid.Empty, "Test", "Test", DateTime.UtcNow.AddHours(1));
            });

            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                var room = new Room(Guid.NewGuid(), string.Empty, string.Empty, DateTime.UtcNow.AddHours(1));
            });

            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                var room = new Room(Guid.NewGuid(), string.Empty, string.Empty, DateTime.UtcNow.AddHours(1));
            });

            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                var room = new Room(Guid.NewGuid(), "Test", "Test", DateTime.UtcNow);
            });
        }

        [Fact(DisplayName = "Room Should Close With Success")]
        [Trait("Domain", "Room Entity Tests")]
        public void Room_CloseRoom_ShouldUpdateRoomStatusToClose()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", DateTime.UtcNow.AddHours(1));

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
            var room = new Room(Guid.NewGuid(), "Test", "Test", DateTime.UtcNow.AddHours(1));
            room.CloseRoom();

            // Act & Assert
            Assert.Throws<DomainException>(room.CloseRoom);
        }

        [Fact(DisplayName = "Room Should Update Plan and Max Participant Quantity")]
        [Trait("Domain", "Room Entity Tests")]
        public void Room_CloseRoom_ShouldUpdatePlanWithSuccess()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", DateTime.UtcNow.AddHours(1));

            // Act
            room.SetPlan(ERoomPlan.Premium);

            // Assert
            Assert.Equal(ERoomPlan.Premium, room.Plan);
            Assert.Equal(Room.MAX_PREMIUM_PARTICIPANTS, room.RoomSpecification.MaxParticipantsNumber);
        }

        [Fact(DisplayName = "Room Should Update Plan and Max Participant Quantity")]
        [Trait("Domain", "Room Entity Tests")]
        public void Rooms_UpdateEndDate_ShouldUpdateEndDate()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", DateTime.UtcNow.AddHours(1));
            var endDate = DateTime.UtcNow.AddHours(10);

            // Act
            room.UpdateEndDate(endDate);

            // Assert
            Assert.True(room.EndDate.HasValue);
            Assert.Equal(endDate, room.EndDate.Value);
        }

        [Fact(DisplayName = "Room Should Update Plan and Max Participant Quantity")]
        [Trait("Domain", "Room Entity Tests")]
        public void Rooms_UpdateEndDate_ShouldThrowDomainExceptionWhenEndDateIsLessThanStartDate()
        {
            // Arrange
            var room = new Room(Guid.NewGuid(), "Test", "Test", DateTime.UtcNow.AddHours(1));
            var endDate = room.StartDate.AddMinutes(-1);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                room.UpdateEndDate(endDate);
            });
        }
    }
}
