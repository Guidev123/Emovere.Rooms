namespace Rooms.Domain.Interfaces.Services
{
    public interface IAspNetUser
    {
        Task<Guid?> GetUserIdAsync();
    }
}