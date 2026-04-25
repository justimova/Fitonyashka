using Fitonyashka.InfrastructureLayer.Interfaces;
using Fytonyashka.DTOs;
using Fytonyashka.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Fitonyashka.InfrastructureLayer;

public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor, IUserService userService) {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    public UserDto GetCurrentUser() {
        var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (username.IsNullOrEmpty()) {
            return null;
        }
        var userDto = _userService.GetByUsername(username);
        return userDto;
    }

    public int? GetCurrentUserId() {
        var userDto = GetCurrentUser();
        return userDto?.Id;
    }
}
