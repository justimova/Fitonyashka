using Fitonyashka.DataAccessLayer.Entities;

namespace Fitonyashka.DataAccessLayer.Repositories;

public interface IUserRepository
{
    List<UserEntity> GetAll();
    void Delete(int id);
    void Add(UserEntity entity);
    bool IsExist(Func<UserEntity, bool> isEqualFunc);
    void Update(UserEntity entity);
    UserEntity GetById(int id);
}

internal class UserRepository : JsonFileRepository<UserEntity>, IUserRepository
{
    protected override string GetEntityName() {
        return "user";
    }
}