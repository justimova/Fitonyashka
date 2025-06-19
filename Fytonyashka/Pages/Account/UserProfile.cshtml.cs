using Fytonyashka.DTOs;
using Fytonyashka.Pages.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Account
{
    public class UserProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly IStaticFilePublisher _staticFilePublisher;
        
        [BindProperty]
        public IFormFile? AvatarFile { get; set; }

        [BindProperty]
        public UserProfileInputModel UserProfileInput { get; set; } = new UserProfileInputModel();

        public UserProfileModel(IUserService userService, IFileService fileService, IStaticFilePublisher staticFilePublisher) {
            _userService = userService;
            _fileService = fileService;
            _staticFilePublisher = staticFilePublisher;
        }
        
        public IActionResult OnGet() { 
            string username = HttpContext.Session.GetString("Username");
            var userDto = _userService.GetByUsername(username);
            if (userDto == null) {
                return NotFound(); // TODO: #2
            }
            UserProfileInput = new UserProfileInputModel {
                Id = userDto.Id,
                Username = userDto.UserName,
                Email = userDto.Email,
                Birthday = userDto.Birthday,
                FirstName = userDto.FirstName,
                Height = userDto.Height,
                AvatarFileName = userDto.AvatarFileName
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            string fileName = UserProfileInput.AvatarFileName;
            if (AvatarFile != null) {
                string avatarPath = await _fileService.UploadFileAsync("UserImages", UserProfileInput.Id, AvatarFile);
                _staticFilePublisher.Publish(avatarPath, "UserImages");
                if (!string.IsNullOrEmpty(fileName)) {
                    await _fileService.DeleteFileAsync("UserImages", fileName);
                    _staticFilePublisher.Delete(fileName, "UserImages");
                }
                fileName = Path.GetFileName(avatarPath);
            }

            _userService.Update(new UserDto {
                Id = UserProfileInput.Id,
                UserName = UserProfileInput.Username,
                Email = UserProfileInput.Email,
                FirstName = UserProfileInput.FirstName,
                Birthday = UserProfileInput.Birthday,
                Height = UserProfileInput.Height,
                AvatarFileName = fileName
            });

            if (!string.IsNullOrEmpty(fileName)) {
                HttpContext.Session.SetString("AvatarFileName", fileName);
            } else {
                HttpContext.Session?.Remove("AvatarFileName");
            }
            return RedirectToPage("/Account/UserProfile");
        }

        public async Task<IActionResult> OnPostDeleteAvatarAsync() {
            await _fileService.DeleteFileAsync("UserImages", UserProfileInput.AvatarFileName);
            _userService.RemoveAvatar(UserProfileInput.Id);
            _staticFilePublisher.Delete(UserProfileInput.AvatarFileName, "UserImages");
            return RedirectToPage("/Account/UserProfile");
        }
    }
}