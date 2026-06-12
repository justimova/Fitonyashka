using Fitonyashka.BusinessLogicLayer.Services.Interfaces;
using Fitonyashka.DataAccessLayer.Entities;
using Fitonyashka.DataAccessLayer.Repositories;
using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private static readonly Dictionary<Type, Func<UserEntity, IUserModel, UserEntity>> _mapDelegates = new() {
        { typeof(UserModel), (e, d) => Map(e, (UserModel)d) },
        { typeof(UserProfileModel), (e, d) => Map(e, (UserProfileModel)d) },
    };

    public UserService(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public ResultModel ChangePassword(int id, string newPassword) {
        var entity = _userRepository.GetById(id);
        entity.Password = newPassword;
        try {
            _userRepository.Update(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to change password");
        }
    }

    public bool CheckPassword(int id, string password) {
        var entity = _userRepository.GetById(id);

        return entity?.Password == password;
    }

    public ResultModel Create(UserModel userDto) {
        if (_userRepository.IsExist(u => u.UserName == userDto.UserName)) {
            return ResultModel.CreateFailedResult("A user with this username already exists");
        }
        if (_userRepository.IsExist(u => u.Email == userDto.Email)) {
            return ResultModel.CreateFailedResult("A user with this email already exists");
        }
        try {
            var entity = Map(userDto);
            _userRepository.Add(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to create user");
        }
    }

    public ResultModel Delete(int id) {
        try {
            _userRepository.Delete(id);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to delete user");
        }
    }

    public List<UserModel> GetAll() {
        var entities = _userRepository.GetAll().OrderByDescending(u => u.UserName);

        return entities.Select(Map).ToList();
    }

    public UserModel GetById(int id) {
        var entity = _userRepository.GetAll().FirstOrDefault(u => u.Id == id);

        return Map(entity);
    }

    public UserModel GetByUsername(string username) {
        var entity = _userRepository.GetAll().FirstOrDefault(u => u.UserName == username);

        return Map(entity);
    }

    public bool Login(string username, string password) {
        UserModel user = GetByUsername(username);
        if (user == null) {
            return false;
        }
        if (user.Password == password) {
            return true;
        }

        return false;
    }

    public ResultModel RemoveAvatar(int id) {
        var result = UpdateAvatar(id, null);
        if (!result.IsSuccess) {
            result.ErrorMessage = "Failed to remove avatar";
        }

        return result;
    }

    private ResultModel Update<TDto>(TDto userDto) where TDto : class, IUserModel {
        if (userDto.Id <= 0) {
            return ResultModel.CreateFailedResult("A user isn't exist or access denied");
        }
        if (_userRepository.IsExist(u => u.Id != userDto.Id && u.UserName == userDto.UserName)) {
            return ResultModel.CreateFailedResult("A user with this username already exists");
        }
        if (_userRepository.IsExist(u => u.Id != userDto.Id && u.Email == userDto.Email)) {
            return ResultModel.CreateFailedResult("A user with this email already exists");
        }
        var entity = _userRepository.GetById(userDto.Id);
        var map = _mapDelegates.GetValueOrDefault(typeof(TDto));
        try {
            map?.Invoke(entity, userDto);
            _userRepository.Update(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to update user account");
        }
    }

    public ResultModel Update(UserModel userDto) {
        return Update<UserModel>(userDto);
    }

    public ResultModel Update(UserProfileModel userProfileDto) {
        return Update<UserProfileModel>(userProfileDto);
    }

    public ResultModel UpdateAvatar(int id, string avatarFileName) {
        var entity = _userRepository.GetById(id);
        entity.AvatarFileName = avatarFileName;
        try {
            _userRepository.Update(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to update avatar");
        }
    }

    public ResultModel UpdateDateRange(int id, int selectedPeriodId) {
        var entity = _userRepository.GetById(id);
        entity.SelectedDateRangeId = selectedPeriodId;
        try {
            _userRepository.Update(entity);

            return ResultModel.CreateSuccessResult();
        } catch {
            return ResultModel.CreateFailedResult("Failed to update default period");
        }
    }

    private UserModel Map(UserEntity entity) => new UserModel {
        Id = entity.Id,
        UserName = entity.UserName,
        Email = entity.Email,
        Password = entity.Password,
        FirstName = entity.FirstName,
        Birthday = entity.Birthday,
        Gender = entity.Gender,
        Height = entity.Height,
        AvatarFileName = entity.AvatarFileName,
        SelectedDateRangeId = entity.SelectedDateRangeId
    };

    private UserEntity Map(UserModel dto) => new UserEntity {
        Id = dto.Id,
        UserName = dto.UserName,
        Email = dto.Email,
        Password = dto.Password,
        FirstName = dto.FirstName,
        Birthday = dto.Birthday,
        Gender = dto.Gender,
        Height = dto.Height,
        AvatarFileName = dto.AvatarFileName,
        SelectedDateRangeId = dto.SelectedDateRangeId
    };

    private static UserEntity Map(UserEntity entity, UserModel dto) {
        entity.UserName = dto.UserName;
        entity.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.Password)) {
            entity.Password = dto.Password;
        }
        return entity;
    }

    private static UserEntity Map(UserEntity entity, UserProfileModel dto) {
        entity.Email = dto.Email;
        entity.FirstName = dto.FirstName;
        entity.Birthday = dto.Birthday;
        entity.Gender = dto.Gender;
        entity.Height = dto.Height;
        return entity;
    }
}
