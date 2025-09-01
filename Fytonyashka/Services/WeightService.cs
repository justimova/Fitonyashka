using Fytonyashka.DataAccessLayer.Entities;
using Fytonyashka.DataAccessLayer.Repositories;
using Fytonyashka.DTOs;
using Fytonyashka.Services.Interfaces;

namespace Fytonyashka.Services;

public class WeightService : IWeightService
{
    private readonly IWeightRepository _weightRepository;

    public WeightService(IWeightRepository weightRepository) {
        _weightRepository = weightRepository;
    }

    public bool Create(WeightDto weightDto) {
        var weight = GetAllByUserId(weightDto.UserId)
            .FirstOrDefault(w => w.Date == weightDto.Date);
        if (weight != null) {
            weightDto.Id = weight.Id;
            return Update(weightDto);
        }
        var entity = Map(weightDto);
        _weightRepository.Add(entity);
        return true;
    }

    public bool Delete(int id) {
        _weightRepository.Delete(id);
        return true;
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

    public bool Update(WeightDto weightDto) {
        var entity = Map(weightDto);
        _weightRepository.Update(entity);
        return true;
    }

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
