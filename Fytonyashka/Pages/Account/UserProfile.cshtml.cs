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
                AvatarPath = userDto.AvatarPath
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                return Page();
            }

            string avatarPath = await _fileService.UploadFileAsync("UserImages", UserProfileInput.Id, AvatarFile);

            _userService.Update(new UserDto {
                Id = UserProfileInput.Id,
                UserName = UserProfileInput.Username,
                Email = UserProfileInput.Email,
                FirstName = UserProfileInput.FirstName,
                Birthday = UserProfileInput.Birthday,
                Height = UserProfileInput.Height,
                AvatarPath = avatarPath
            });

            _staticFilePublisher.Publish(avatarPath, "UserImages");
            return RedirectToPage("/Account/UserProfile");
        }
    }
}