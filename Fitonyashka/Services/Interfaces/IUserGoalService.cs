using Fitonyashka.DTOs;

namespace Fitonyashka.Services.Interfaces;

public interface IUserGoalService
{
    ResultDto Create(UserGoalDto userGoalDto);
    ResultDto Delete(int userId);
    UserGoalDto GetByUserId(int userId);
}