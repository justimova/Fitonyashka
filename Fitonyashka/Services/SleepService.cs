using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.DTOs;
using Fitonyashka.Services.Interfaces;

namespace Fitonyashka.Services;

public class SleepService : ISleepService
{
    private readonly ISleepRepository _sleepRepository;

    public SleepService(ISleepRepository sleepRepository) {
        _sleepRepository = sleepRepository;
    }

    public ResultDto Create(SleepDto sleepDto) {
        if (sleepDto.DateFrom >= sleepDto.DateTo) {
            return ResultDto.CreateFailedResult("Bedtime should be earlier than wake-up time");
        }
        try {
            var entity = Map(sleepDto);
            _sleepRepository.Add(entity);
            return ResultDto.CreateSuccessResult();
        } catch {
            return ResultDto.CreateFailedResult("Failed to add record");
        }
    }

    public ResultDto Delete(int id) {
        try {
            _sleepRepository.Delete(id);
            return ResultDto.CreateSuccessResult();
        } catch {
            return ResultDto.CreateFailedResult("Failed to delete record");
        }
    }

    public List<SleepDto> GetAll() {
        var entities = _sleepRepository.GetAll().OrderByDescending(w => w.DateFrom);
        return entities.Select(Map).ToList();
    }

    public List<SleepDto> GetAllByUserId(int userId) => GetAll().Where(w => w.UserId == userId).ToList();

    public SleepDto GetById(int id) {
        throw new NotImplementedException();
    }

    public SleepDto GetLastByUserId(int userId) {
        throw new NotImplementedException();
    }

    public ResultDto Update(SleepDto sleepDto) {
        throw new NotImplementedException();
    }

    private SleepDto Map(SleepEntity entity) => new SleepDto {
        Id = entity.Id,
        UserId = entity.UserId,
        DateFrom = entity.DateFrom,
        DateTo = entity.DateTo
    };

    private SleepEntity Map(SleepDto dto) => new SleepEntity {
        Id = dto.Id,
        UserId = dto.UserId,
        DateFrom = dto.DateFrom,
        DateTo = dto.DateTo
    };
}
