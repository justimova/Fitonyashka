using Fytonyashka.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public LoginInputModel LoginInput { get; set; } = new LoginInputModel();

        public LoginModel(IUserService userService) {
            _userService = userService;
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (_userService.Login(LoginInput.UserName, LoginInput.Password))
            {
                var userDto = _userService.GetByUsername(LoginInput.UserName);
                HttpContext.Session.SetString("Username", LoginInput.UserName);
                HttpContext.Session.SetString("AvatarFileName", userDto?.AvatarFileName ?? "");
                HttpContext.Session.SetInt32("UserId", userDto?.Id ?? 0);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError("", "You entered wrong username or password");
            return Page();
        }
    }
}