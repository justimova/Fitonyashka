using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.DTOs;
using Fitonyashka.Services.Interfaces;

namespace Fitonyashka.Services;

public class WeightService : IWeightService
{
    private readonly IWeightRepository _weightRepository;

    public WeightService(IWeightRepository weightRepository) {
        _weightRepository = weightRepository;
    }

    public ResultDto Create(WeightDto weightDto) {
        var weight = GetAllByUserId(weightDto.UserId)
            .FirstOrDefault(w => w.Date == weightDto.Date);
        if (weight != null) {
            weightDto.Id = weight.Id;
            return Update(weightDto);
        }
        try {
            var entity = Map(weightDto);
            _weightRepository.Add(entity);
            return ResultDto.CreateSuccessResult();
        } catch {
            return ResultDto.CreateFailedResult("Failed to enter weight");
        }
    }

    public ResultDto Update(WeightDto weightDto) {
        try {
            var entity = Map(weightDto);
            _weightRepository.Update(entity);
            return ResultDto.CreateSuccessResult();
        } catch {
            return ResultDto.CreateFailedResult("Failed to update weight");
        }
    }

    public ResultDto Delete(int id) {
        try {
            _weightRepository.Delete(id);
            return ResultDto.CreateSuccessResult();
        } catch {
            return ResultDto.CreateFailedResult("Failed to delete weight");
        }
    }

    public List<WeightDto> GetAll() {
        var entities = _weightRepository.GetAll().OrderByDescending(w => w.Date);
        return entities.Select(Map).ToList();
    }

    public List<WeightDto> GetAllByUserId(int userId) => GetAll().Where(w => w.UserId == userId).ToList();

    public WeightDto GetById(int id) {
        var entity = _weightRepository.GetAll().FirstOrDefault(e => e.Id == id);
        return Map(entity);
    }

    public WeightDto GetLastByUserId(int userId) => GetAllByUserId(userId).FirstOrDefault();

    private WeightDto Map(WeightEntity entity) => new WeightDto {
        Date = entity.Date,
        Weight = entity.Weight,
        Id = entity.Id,
        UserId = entity.UserId
    };

    private WeightEntity Map(WeightDto dto) => new WeightEntity {
        Date = dto.Date,
        Weight = dto.Weight,
        Id = dto.Id,
        UserId = dto.UserId
    };
}
