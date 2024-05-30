using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WarrAttachmentManagementService.Application.Interfaces;

namespace WarrAttachmentManagementService.API.Services;

public class CurrentUser : ICurrentUser
{
    private readonly ClaimsPrincipal _user;
    private Guid? _userId;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext?.User;
    }

    public Guid Id
    {
        get
        {
            if (_userId.HasValue)
                return _userId.Value;

            var userId = _user?
                .Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?
                .Value;

            _userId = userId is null
                ? throw new InvalidOperationException("UserId is null")
                : Guid.TryParse(userId, out Guid userIdGuid)
                    ? userIdGuid
                    : throw new InvalidOperationException("UserId is incorrect");

            return _userId.Value;
        }
    }

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) ??
        throw new InvalidOperationException("Unauthenticated");
}
