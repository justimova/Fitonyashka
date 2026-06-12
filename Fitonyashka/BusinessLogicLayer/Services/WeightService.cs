using Fitonyashka.BusinessLogicLayer.Services.Interfaces;
using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services;

public class WeightService : IWeightService
{
    private readonly IWeightRepository _weightRepository;

    public WeightService(IWeightRepository weightRepository) {
        _weightRepository = weightRepository;
    }

    public ResultModel Create(WeightModel weightDto) {
        var weight = GetAllByUserId(weightDto.UserId)
            .FirstOrDefault(w => w.Date == weightDto.Date);
        if (weight != null) {
            weightDto.Id = weight.Id;

            return Update(weightDto);
        }
        try {
            var entity = Map(weightDto);
            _weightRepository.Add(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to enter weight");
        }
    }

    public ResultModel Update(WeightModel weightDto) {
        var entity = _weightRepository.GetAll().FirstOrDefault(g => g.Id == weightDto.Id);
        if (entity == null) {
            return ResultModel.CreateFailedResult("Failed to update weight");
        }
        try {
            entity = Map(entity, weightDto);
            _weightRepository.Update(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to update weight");
        }
    }

    public ResultModel Delete(int id) {
        try {
            _weightRepository.Delete(id);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to delete weight");
        }
    }

    public List<WeightModel> GetAll() {
        var entities = _weightRepository.GetAll().OrderByDescending(w => w.Date);

        return entities.Select(Map).ToList();
    }

    public List<WeightModel> GetAllByUserId(int userId) => GetAll().Where(w => w.UserId == userId).ToList();

    public WeightModel GetById(int id) {
        var entity = _weightRepository.GetAll().FirstOrDefault(e => e.Id == id);

        return Map(entity);
    }

    public WeightModel GetLastByUserId(int userId) => GetAllByUserId(userId).FirstOrDefault();

    private WeightModel Map(WeightEntity entity) => new WeightModel {
        Date = entity.Date,
        Weight = entity.Weight,
        Id = entity.Id,
        UserId = entity.UserId,
    };

    private WeightEntity Map(WeightModel dto) => new WeightEntity {
        Date = dto.Date,
        Weight = dto.Weight,
        Id = dto.Id,
        UserId = dto.UserId,
    };

    private WeightEntity Map(WeightEntity entity, WeightModel dto) {
        entity.Date = dto.Date;
        entity.Weight = dto.Weight;

        return entity;
    }
}
