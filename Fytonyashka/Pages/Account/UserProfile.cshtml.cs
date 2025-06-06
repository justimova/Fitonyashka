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
        [BindProperty]
        public UserProfileInputModel UserPrifileInput { get; set; } = new UserProfileInputModel();

        public UserProfileModel(IUserService userService) {
            _userService = userService;
        }
        
        public IActionResult OnGet() { 
            string username = HttpContext.Session.GetString("Username");
            var userDto = _userService.GetByUsername(username);
            if (userDto == null) {
                return NotFound(); // TODO: #2
            }
            UserPrifileInput = new UserProfileInputModel {
                Id = userDto.Id,
                Username = userDto.UserName,
                Email = userDto.Email,
                Birthday = userDto.Birthday,
                FirstName = userDto.FirstName,
                Height = userDto.Height
            };
            return Page();
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }
            _userService.Update(new UserDto { // TODO: #2
                Id = UserPrifileInput.Id,
                UserName = UserPrifileInput.Username,
                Email = UserPrifileInput.Email,
                FirstName = UserPrifileInput.FirstName,
                Birthday = UserPrifileInput.Birthday,
                Height = UserPrifileInput.Height
            });
            return RedirectToPage("/Account/UserProfile");
        }
    }
}