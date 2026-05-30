using Fitonyashka.DTOs;

namespace Fitonyashka.Services.Interfaces;

public interface IGoalService
{
    ResultDto Create(GoalDto goalDto);
    ResultDto Delete(int goalId);
    GoalDto GetActiveGoalByUserId(int userId);
}