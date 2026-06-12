using Fitonyashka.BusinessLogicLayer.Services.Interfaces;
using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services;

internal class WeightDateRangeService : IWeightDateRangeService
{
    private readonly IWeightDateRangeRepository _weightDateRangeRepository;

    public WeightDateRangeService(IWeightDateRangeRepository weightDateRangeRepository) {
        _weightDateRangeRepository = weightDateRangeRepository;
    }

    public List<DateRangeModel> GetAll() {
        var entities = _weightDateRangeRepository.GetAll().OrderByDescending(d => d.Position);

        return entities.Select(Map).ToList();
    }

    public DateRangeModel GetById(int id) {
        var entity = _weightDateRangeRepository.GetAll().FirstOrDefault(d => d.Id == id);
        if (entity == null) {
            return null;
        }

        return Map(entity);
    }

    private DateRangeModel Map(WeightDateRangeEntity entity) => new DateRangeModel {
        Id = entity.Id,
        Text = entity.Text,
        IsFiltered = entity.IsFiltered,
        FilterNumber = entity.FilterNumber,
        FilterDateRange = entity.FilterDateRange,
        Position = entity.Position
    };
}
