using System.ComponentModel;
using System.Reflection;

namespace Rooms.Domain.Enums
{
    public enum EReportMessages
    {
        [Description("Error: Validation Error.")]
        VALIDATION_ERROR,
        [Description("Error: Participant cannot be added to the room.")]
        PARTICIPANT_CANNOT_BE_ADDED_TO_ROOM,
        [Description("Error: Fail to persist data.")]
        FAIL_TO_PERSIST_DATA,
        [Description("Success: Created Room With Success.")]
        ROOM_CREATED_WITH_SUCCESS,
    }

    public static class EnumExtension
    {
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString()) ?? default!;

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[]
                ?? throw new ArgumentNullException();

            return attributes is not null && attributes.Length != 0 ? attributes.First().Description : value.ToString();
        }
    }
}
