using Fitonyashka.DataAccessLayer.Entities;

namespace Fitonyashka.DataAccessLayer.Repositories;

public interface IWeightDateRangeRepository
{
    List<WeightDateRangeEntity> GetAll();
}

internal class WeightDateRangeRepository : JsonFileRepository<WeightDateRangeEntity>, IWeightDateRangeRepository
{
    protected override string GetEntityName() => "weightDateRange";
}
