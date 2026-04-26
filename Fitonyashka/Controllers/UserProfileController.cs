using Fitonyashka.InfrastructureLayer.Interfaces;
using Fitonyashka.ViewModels;
using Fitonyashka.ViewModels.UserProfile;
using Fytonyashka.DTOs;
using Fytonyashka.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Fitonyashka.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserProfileController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICurrentUserContext _currentUserContext;

    public UserProfileController(IUserService userService, ICurrentUserContext currentUserContext) {
        _userService = userService;
        _currentUserContext = currentUserContext;
    }

    [HttpGet]
    [Route("currentUser")]
    [Authorize]
    public ActionResult<UserInfoViewModel> GetCurrentUser() {
        UserDto userDto = _currentUserContext.GetCurrentUser();
        if (userDto == null) {
            return Unauthorized();
        }

        return new UserInfoViewModel {
            UserId = userDto.Id,
            Email = userDto.Email,
            Username = userDto.UserName,
            FirstName = userDto.FirstName,
            Birthday = DateOnly.FromDateTime(userDto.Birthday),
            Gender = userDto.Gender,
            Height = userDto.Height,
            Weight = userDto.Weight,
            AvatarFileName = userDto.AvatarFileName,
        };
    }

    [HttpPut]
    [Authorize]
    public ActionResult<ResultViewModel> UpdateUserProfile([FromBody] UserUpdateViewModel updateViewModel) {
        UserProfileDto userProfileDto = new UserProfileDto {
            Id = updateViewModel.UserId,
            Email = updateViewModel.Email,
            FirstName = updateViewModel.FirstName,
            Birthday = updateViewModel.Birthday.ToDateTime(TimeOnly.MinValue),
            Gender = updateViewModel.Gender,
            Height = updateViewModel.Height,
            Weight = updateViewModel.Weight,
        };

        var resultDto = _userService.Update(userProfileDto);

        var resultViewModel = new ResultViewModel {
            ErrorMessage = resultDto.ErrorMessage,
            IsSuccess = resultDto.IsSuccess,
        };
        return Ok(resultViewModel);
    }
}
