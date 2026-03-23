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

    public UserProfileController(IUserService userService) {
        _userService = userService;
    }

    [HttpGet]
    [Route("currentUser")]
    [Authorize]
    public ActionResult<UserInfoViewModel> GetCurrentUser() {
        var username = User.Identity?.Name; // get username from token claim
        if (username.IsNullOrEmpty()) {
            return Unauthorized();
        }
        var userDto = _userService.GetByUsername(username);
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
