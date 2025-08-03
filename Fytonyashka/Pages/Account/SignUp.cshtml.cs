using Fytonyashka.DataModels;
using Fytonyashka.DTOs;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public SignUpInputModel SignUpInput { get; set; } = new SignUpInputModel();

        public SignUpModel(IUserService userService) {
            _userService = userService;
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            var userDto = new UserDto {
                UserName = SignUpInput.UserName,
                Email = SignUpInput.Email,
                Password = SignUpInput.Password
            };
            if (_userService.Create(userDto)) {
                userDto = _userService.GetByUsername(SignUpInput.UserName);
                HttpContext.Session.SetString("Username", SignUpInput.UserName);
                HttpContext.Session.SetString("AvatarFileName", userDto?.AvatarFileName ?? "");
                HttpContext.Session.SetInt32("UserId", userDto?.Id ?? 0);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError("", "You entered wrong username or password");
            return Page();
        }
    }
}
