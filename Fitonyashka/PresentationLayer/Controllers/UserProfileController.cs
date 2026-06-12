using Fitonyashka.InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitonyashka.Models;
using Fitonyashka.PresentationLayer.ViewModels;
using Fitonyashka.PresentationLayer.ViewModels.UserProfile;
using Fitonyashka.BusinessLogicLayer.Services.Interfaces;

namespace Fitonyashka.PresentationLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IWeightService _weightService;

    public UserProfileController(IUserService userService, ICurrentUserContext currentUserContext, IWeightService weightService) {
        _userService = userService;
        _currentUserContext = currentUserContext;
        _weightService = weightService;
    }

    [HttpGet]
    [Route("currentUser")]
    public ActionResult<UserInfoViewModel> GetCurrentUser() {
        UserModel userDto = _currentUserContext.GetCurrentUser();
        if (userDto == null) {
            return Unauthorized();
        }
        var weightDto = _weightService.GetLastByUserId(userDto.Id);

        return new UserInfoViewModel {
            UserId = userDto.Id,
            Email = userDto.Email,
            Username = userDto.UserName,
            FirstName = userDto.FirstName,
            Birthday = DateOnly.FromDateTime(userDto.Birthday),
            Gender = userDto.Gender,
            Height = userDto.Height,
            Weight = weightDto?.Weight ?? 0m,
            AvatarFileName = userDto.AvatarFileName,
        };
    }

    [HttpPut]
    public ActionResult<ResultViewModel> UpdateUserProfile([FromBody] UserUpdateViewModel updateViewModel) {
        UserProfileModel userProfileDto = new UserProfileModel {
            Id = updateViewModel.UserId,
            Email = updateViewModel.Email,
            FirstName = updateViewModel.FirstName,
            Birthday = updateViewModel.Birthday.ToDateTime(TimeOnly.MinValue),
            Gender = updateViewModel.Gender,
            Height = updateViewModel.Height,
        };
        var resultDto = _userService.Update(userProfileDto);
        var resultViewModel = new ResultViewModel {
            ErrorMessage = resultDto.ErrorMessage,
            IsSuccess = resultDto.IsSuccess,
        };

        return Ok(resultViewModel);
    }
}
