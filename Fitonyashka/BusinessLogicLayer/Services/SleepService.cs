using Fitonyashka.BusinessLogicLayer.Services.Interfaces;
using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services;

public class SleepService : ISleepService
{
    private readonly ISleepRepository _sleepRepository;

    public SleepService(ISleepRepository sleepRepository) {
        _sleepRepository = sleepRepository;
    }

    public ResultModel Create(SleepModel sleepDto) {
        if (sleepDto.DateFrom >= sleepDto.DateTo) {
            return ResultModel.CreateFailedResult("Bedtime should be earlier than wake-up time");
        }
        try {
            var entity = Map(sleepDto);
            _sleepRepository.Add(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to add record");
        }
    }

    public ResultModel Delete(int id) {
        try {
            _sleepRepository.Delete(id);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to delete record");
        }
    }

    public List<SleepModel> GetAll() {
        var entities = _sleepRepository.GetAll().OrderByDescending(w => w.DateFrom);

        return entities.Select(Map).ToList();
    }

    public List<SleepModel> GetAllByUserId(int userId) => GetAll().Where(w => w.UserId == userId).ToList();

    public SleepModel GetById(int id) {
        throw new NotImplementedException();
    }

    public SleepModel GetLastByUserId(int userId) {
        throw new NotImplementedException();
    }

    public ResultModel Update(SleepModel sleepDto) {
        throw new NotImplementedException();
    }

    private SleepModel Map(SleepEntity entity) => new SleepModel {
        Id = entity.Id,
        UserId = entity.UserId,
        DateFrom = entity.DateFrom,
        DateTo = entity.DateTo
    };

    private SleepEntity Map(SleepModel dto) => new SleepEntity {
        Id = dto.Id,
        UserId = dto.UserId,
        DateFrom = dto.DateFrom,
        DateTo = dto.DateTo
    };
}
