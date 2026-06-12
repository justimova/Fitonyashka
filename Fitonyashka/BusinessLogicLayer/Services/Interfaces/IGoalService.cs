using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services.Interfaces;

public interface IGoalService
{
    ResultModel Create(GoalModel goalDto);
    ResultModel Update(GoalModel goalDto);
    ResultModel Delete(int goalId);
    GoalModel GetActiveGoalByUserId(int userId);
    GoalModel GetGoalById(int id);
    bool CompleteIfNeeded(int userId, decimal weight);
}
