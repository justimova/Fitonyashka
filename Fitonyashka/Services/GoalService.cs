using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.DTOs;
using Fitonyashka.Services.Interfaces;

namespace Fitonyashka.Services;

public class GoalService : IGoalService
{
    private readonly IGoalRepository _goalRepository;

    public GoalService(IGoalRepository goalRepository) {
        _goalRepository = goalRepository;
    }

    public ResultDto Create(GoalDto goalDto) {
        try {
            var entity = Map(goalDto);
            _goalRepository.Add(entity);

            return ResultDto.CreateSuccessResult();
        } catch {
            return ResultDto.CreateFailedResult("Failed to set goal");
        }
    }

    public ResultDto Delete(int goalId) {
        var entity = _goalRepository.GetAll().FirstOrDefault(g => g.Id == goalId);
        if (entity == null) {
            return ResultDto.CreateFailedResult("Failed to delete goal");
        }
        try {
            _goalRepository.Delete(entity.Id);
            return ResultDto.CreateSuccessResult();
        } catch {
            return ResultDto.CreateFailedResult("Failed to delete goal");
        }
    }

    public GoalDto GetActiveGoalByUserId(int userId) {
        var entity = _goalRepository.GetAll().FirstOrDefault(g => g.UserId == userId && g.EndDate == null);
        if (entity == null) {
            return null;
        }

        return Map(entity);
    }

    private GoalDto Map(GoalEntity entity) => new GoalDto {
        StartDate = entity.StartDate,
        TargetWeight = entity.TargetWeight,
        InitialWeight = entity.InitialWeight,
        Id = entity.Id,
        UserId = entity.UserId
    };

    private GoalEntity Map(GoalDto dto) => new GoalEntity {
        StartDate = dto.StartDate,
        TargetWeight = dto.TargetWeight,
        InitialWeight = dto.InitialWeight,
        Id = dto.Id,
        UserId = dto.UserId
    };
}
