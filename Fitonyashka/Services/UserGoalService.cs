using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.DTOs;
using Fitonyashka.Services.Interfaces;

namespace Fitonyashka.Services;

public class UserGoalService : IUserGoalService
{
    private readonly IUserGoalRepository _userGoalRepository;

    public UserGoalService(IUserGoalRepository userGoalRepository) {
        _userGoalRepository = userGoalRepository;
    }

    public ResultDto Create(UserGoalDto userGoalDto) {
        try {
            var entity = Map(userGoalDto);
            _userGoalRepository.Add(entity);
            return ResultDto.CreateSuccessResult();
        } catch {
        
            return ResultDto.CreateFailedResult("Failed to set goal");
        }
    }

    public ResultDto Delete(int userId) {
        var entity = _userGoalRepository.GetAll().FirstOrDefault(g => g.UserId == userId);
        if (entity == null) {
            return ResultDto.CreateFailedResult("Failed to delete goal");
        }
        try {
            _userGoalRepository.Delete(entity.Id);
            return ResultDto.CreateSuccessResult();
        } catch {
            return ResultDto.CreateFailedResult("Failed to delete goal");
        }
    }

    public UserGoalDto GetByUserId(int userId) {
        var entity = _userGoalRepository.GetAll().FirstOrDefault(g => g.UserId == userId);
        if (entity == null) {
            return null;
        }
        return Map(entity);
    }

    private UserGoalDto Map(UserGoalEntity entity) => new UserGoalDto {
        StartDate = entity.StartDate,
        Weight = entity.Weight,
        InitialWeight = entity.InitialWeight,
        Id = entity.Id,
        UserId = entity.UserId
    };

    private UserGoalEntity Map(UserGoalDto dto) => new UserGoalEntity {
        StartDate = dto.StartDate,
        Weight = dto.Weight,
        InitialWeight = dto.InitialWeight,
        Id = dto.Id,
        UserId = dto.UserId
    };
}
