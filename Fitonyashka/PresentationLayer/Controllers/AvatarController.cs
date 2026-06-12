using Fitonyashka.BusinessLogicLayer.Services;
using Fitonyashka.BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Fitonyashka.PresentationLayer.Controllers;

[ApiController]
[Route("api/users/me/avatar")]
public class AvatarController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IUserService _userService;
    private readonly IFileService _fileService;
    private readonly IStaticFilePublisher _staticFilePublisher;
    private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
    private static readonly HashSet<string> AllowedTypes = new() {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    public AvatarController(IWebHostEnvironment env, IUserService userService, IFileService fileService,
                IStaticFilePublisher staticFilePublisher) {
        _env = env;
        _userService = userService;
        _fileService = fileService;
        _staticFilePublisher = staticFilePublisher;
    }

    [Authorize]
    [HttpPost]
    [RequestSizeLimit(MaxFileSize)]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file, CancellationToken ct) {
        if (file == null || file.Length == 0) {
            return BadRequest("File isn't uploaded.");
        }

        if (file.Length > MaxFileSize) {
            return BadRequest("File size is too big.");
        }
            
        if (!AllowedTypes.Contains(file.ContentType)) {
            return BadRequest("Invalid file type.");
        }

        var username = User.Identity?.Name; // get username from token claim
        if (username.IsNullOrEmpty()) {
            return Unauthorized();
        }

        var userDto = _userService.GetByUsername(username);
        if (userDto == null) {
            return Unauthorized();
        }

        string avatarPath = await _fileService.UploadFileAsync("UserImages", userDto.Id, file);
        _staticFilePublisher.Publish(avatarPath, "UserImages");

        if (!string.IsNullOrEmpty(userDto.AvatarFileName)) {
            await _fileService.DeleteFileAsync("UserImages", userDto.AvatarFileName);
            _staticFilePublisher.Delete(userDto.AvatarFileName, "UserImages");
        }

        string fileName = Path.GetFileName(avatarPath);

        _userService.UpdateAvatar(userDto.Id, fileName);

        return Ok(new { fileName });
    }
}
