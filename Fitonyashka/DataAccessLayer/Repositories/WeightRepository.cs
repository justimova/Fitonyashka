using Fytonyashka.DataAccessLayer.Entities;

namespace Fytonyashka.DataAccessLayer.Repositories;

public interface IWeightRepository
{
    List<WeightEntity> GetAll();
    void Add(WeightEntity entity);
    void Update(WeightEntity entity);
    void Delete(int id);
}

internal class WeightRepository : JsonFileRepository<WeightEntity>, IWeightRepository
{
    protected override string GetEntityName() {
        return "weight";
    }
}
