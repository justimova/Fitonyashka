using Fytonyashka.DataAccessLayer.Entities;
using Fytonyashka.DataAccessLayer.Repositories;
using Fytonyashka.DTOs;
using Fytonyashka.Services.Interfaces;

namespace Fytonyashka.Services;

internal class WeightDateRangeService : IWeightDateRangeService
{
    private readonly IWeightDateRangeRepository _weightDateRangeRepository;

    public WeightDateRangeService(IWeightDateRangeRepository weightDateRangeRepository)
    {
        _weightDateRangeRepository = weightDateRangeRepository;
    }

    public List<DateRangeDto> GetAll()
    {
        var entities = _weightDateRangeRepository.GetAll().OrderByDescending(d => d.Position);
        return entities.Select(Map).ToList();
    }

    public DateRangeDto GetById(int id)
    {
        var entity = _weightDateRangeRepository.GetAll().FirstOrDefault(d => d.Id == id);
        if (entity == null) {
            return null;
        }
        return Map(entity);
    }

    private DateRangeDto Map(WeightDateRangeEntity entity) => new DateRangeDto {
        Id = entity.Id,
        Text = entity.Text,
        IsFiltered = entity.IsFiltered,
        FilterNumber = entity.FilterNumber,
        FilterDateRange = entity.FilterDateRange,
        Position = entity.Position
    };
}
