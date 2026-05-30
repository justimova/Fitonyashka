using Fitonyashka.DataAccessLayer.Entities;

namespace Fitonyashka.DataAccessLayer.Repositories;

public interface IGoalRepository
{
    List<GoalEntity> GetAll();
    void Delete(int id);
    void Add(GoalEntity entity);
}

internal class GoalRepository : JsonFileRepository<GoalEntity>, IGoalRepository
{
    protected override string GetEntityName() {
        return "goal";
    }
}
