using Fytonyashka.DataAccessLayer.Entities;

namespace Fytonyashka.DataAccessLayer.Repositories;

public interface IUserGoalRepository
{
    List<UserGoalEntity> GetAll();
    void Delete(int id);
    void Add(UserGoalEntity entity);
}

internal class UserGoalRepository : JsonFileRepository<UserGoalEntity>, IUserGoalRepository
{
    protected override string GetEntityName() {
        return "userGoal";
    }
}
