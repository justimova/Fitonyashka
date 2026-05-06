using Fitonyashka.DTOs;

namespace Fitonyashka.InfrastructureLayer.Interfaces;

public interface ICurrentUserContext
{
    UserDto GetCurrentUser();
    int? GetCurrentUserId();
}
