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

            var result = _userService.Create(userDto);
            if (!result.IsSuccess) {
                ModelState.AddModelError("", result.ErrorMessage);
                return Page();
            }

            _userService.Login(userDto.UserName, userDto.Email);
            HttpContext.Session.SetString("Username", userDto.UserName);
            HttpContext.Session.SetString("AvatarFileName", userDto?.AvatarFileName ?? "");
            HttpContext.Session.SetInt32("UserId", userDto?.Id ?? 0);
            return RedirectToPage("/Index");
        }
    }
}
