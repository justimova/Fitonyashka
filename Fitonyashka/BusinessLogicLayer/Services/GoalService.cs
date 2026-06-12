using Fitonyashka.BusinessLogicLayer.Services.Interfaces;
using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services;

public class GoalService : IGoalService
{
    private readonly IGoalRepository _goalRepository;

    public GoalService(IGoalRepository goalRepository) {
        _goalRepository = goalRepository;
    }

    public ResultModel Create(GoalModel goalDto) {
        try {
            var entity = Map(goalDto);
            _goalRepository.Add(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to set goal");
        }
    }

    public ResultModel Update(GoalModel goalDto) {
        var entity = _goalRepository.GetAll().FirstOrDefault(g => g.Id == goalDto.Id);
        if (entity == null) {
            return ResultModel.CreateFailedResult("Failed to update goal");
        }
        try {
            entity = Map(entity, goalDto);
            _goalRepository.Update(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to update goal");
        }
    }

    public ResultModel Delete(int goalId) {
        var entity = _goalRepository.GetAll().FirstOrDefault(g => g.Id == goalId);
        if (entity == null) {
            return ResultModel.CreateFailedResult("Failed to delete goal");
        }
        try {
            _goalRepository.Delete(entity.Id);
            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to delete goal");
        }
    }

    public GoalModel GetActiveGoalByUserId(int userId) {
        var entity = _goalRepository.GetAll().FirstOrDefault(g => g.UserId == userId && g.EndDate == null);
        if (entity == null) {
            return null;
        }

        return Map(entity);
    }

    public GoalModel GetGoalById(int id) {
        var entity = _goalRepository.GetAll().FirstOrDefault(g => g.Id == id);
        if (entity == null) {
            return null;
        }

        return Map(entity);
    }

    public bool CompleteIfNeeded(int userId, decimal weight) {
        GoalModel goalModel = GetActiveGoalByUserId(userId);
        if (goalModel == null) {
            return false;
        }
        if (IsGoalGained(goalModel, weight)) {
            Complete(goalModel.Id);

            return true;
        }

        return false;
    }

    private bool IsGoalGained(GoalModel goalModel, decimal weight) {
        bool IsWeightLoosing = goalModel.InitialWeight >= goalModel.TargetWeight;
        if (IsWeightLoosing) {
            return weight <= goalModel.TargetWeight;
        }

        return weight >= goalModel.TargetWeight;
    }

    private ResultModel Complete(int id) {
        var entity = _goalRepository.GetAll().FirstOrDefault(g => g.Id == id);
        if (entity == null) {
            return ResultModel.CreateFailedResult("Failed to complete goal");
        }
        try {
            entity.EndDate = DateTime.UtcNow;
            _goalRepository.Update(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to complete goal");
        }
    }

    private GoalModel Map(GoalEntity entity) => new GoalModel {
        StartDate = entity.StartDate,
        TargetWeight = entity.TargetWeight,
        InitialWeight = entity.InitialWeight,
        Id = entity.Id,
        UserId = entity.UserId,
    };

    private GoalEntity Map(GoalModel dto) => new GoalEntity {
        StartDate = dto.StartDate,
        TargetWeight = dto.TargetWeight,
        InitialWeight = dto.InitialWeight,
        Id = dto.Id,
        UserId = dto.UserId,
    };

    private GoalEntity Map(GoalEntity entity, GoalModel dto) {
        entity.TargetWeight = dto.TargetWeight;

        return entity;
    }
}
