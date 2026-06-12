using Fitonyashka.Models;

namespace Fitonyashka.InfrastructureLayer.Interfaces;

public interface ICurrentUserContext
{
    UserModel GetCurrentUser();
    int? GetCurrentUserId();
}
