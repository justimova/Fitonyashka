using Fytonyashka.DataAccessLayer.Entities;

namespace Fytonyashka.DataAccessLayer.Repositories;

public interface ISleepRepository
{
    List<SleepEntity> GetAll();
    void Add(SleepEntity entity);
    void Update(SleepEntity entity);
    void Delete(int id);
}

internal class SleepRepository : JsonFileRepository<SleepEntity>, ISleepRepository
{
    protected override string GetEntityName() {
        return "sleep";
    }
}
