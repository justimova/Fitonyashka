using Fitonyashka.DTOs;

namespace Fitonyashka.Services.Interfaces;

public interface IUserService
{
    ResultDto Create(UserDto userDto);
    bool Login(string username, string password);
    List<UserDto> GetAll();
    ResultDto Delete(int id);
    UserDto GetById(int id);
    ResultDto Update(UserDto userDto);
    ResultDto Update(UserProfileDto userDto);
    UserDto GetByUsername(string username);
    ResultDto RemoveAvatar(int id);
    ResultDto UpdateAvatar(int id, string avatarFileName);
    bool CheckPassword(int id, string password);
    ResultDto ChangePassword(int id, string newPassword);
    ResultDto UpdateDateRange(int id, int selectedPeriodId);
}
