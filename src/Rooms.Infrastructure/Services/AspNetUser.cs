using Microsoft.AspNetCore.Http;
using Rooms.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Rooms.Infrastructure.Services
{
    public sealed class AspNetUser(IHttpContextAccessor httpContextAccessor) : IAspNetUser
    {
        private readonly ClaimsPrincipal _claims = httpContextAccessor.HttpContext!.User;

        public Task<Guid?> GetUserIdAsync()
        {
            var userIdClaim = _claims?.FindFirst("sub")?.Value ?? _claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
                return Task.FromResult<Guid?>(userId);

            return Task.FromResult<Guid?>(null);
        }
    }
}