using Fitonyashka.DataAccessLayer.Entities;

namespace Fitonyashka.DataAccessLayer.Repositories;

public interface IGoalRepository
{
    void Add(GoalEntity entity);
    void Update(GoalEntity entity);
    void Delete(int id);
    List<GoalEntity> GetAll();
}

internal class GoalRepository : JsonFileRepository<GoalEntity>, IGoalRepository
{
    protected override string GetEntityName() => "goal";
}
