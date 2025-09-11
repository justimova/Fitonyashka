using Fytonyashka.DataAccessLayer.Entities;

namespace Fytonyashka.DataAccessLayer.Repositories;

public interface IWeightDateRangeRepository
{
    List<WeightDateRangeEntity> GetAll();
}

internal class WeightDateRangeRepository : JsonFileRepository<WeightDateRangeEntity>, IWeightDateRangeRepository
{
    protected override string GetEntityName() {
        return "weightDateRange";
    }
}
