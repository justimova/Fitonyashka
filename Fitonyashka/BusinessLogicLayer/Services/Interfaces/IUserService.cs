using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services.Interfaces;

public interface IUserService
{
    ResultModel Create(UserModel userDto);
    bool Login(string username, string password);
    List<UserModel> GetAll();
    ResultModel Delete(int id);
    UserModel GetById(int id);
    ResultModel Update(UserModel userDto);
    ResultModel Update(UserProfileModel userDto);
    UserModel GetByUsername(string username);
    ResultModel RemoveAvatar(int id);
    ResultModel UpdateAvatar(int id, string avatarFileName);
    bool CheckPassword(int id, string password);
    ResultModel ChangePassword(int id, string newPassword);
    ResultModel UpdateDateRange(int id, int selectedPeriodId);
}
