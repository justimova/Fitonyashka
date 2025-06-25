using Fytonyashka.Pages.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Account
{
	public class SettingsModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public ChangePasswordInputModel ChangePasswordInput { get; set; } = new ChangePasswordInputModel();

        public SettingsModel(IUserService userService) {
            _userService = userService;
        }

        public IActionResult OnGet() {
            string username = HttpContext.Session.GetString("Username");
            var userDto = _userService.GetByUsername(username);
            if (userDto == null) {
                return NotFound(); // TODO: #2
            }

            ChangePasswordInput.Id = userDto.Id;

            return Page();
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (!_userService.CheckPassword(ChangePasswordInput.Id, ChangePasswordInput.Password)) {
                ModelState.AddModelError("ChangePasswordInput.Password", "Current password is incorrect.");
                return Page();
            }

            _userService.ChangePassword(ChangePasswordInput.Id, ChangePasswordInput.NewPassword);
            return RedirectToPage();
        }
    }
}
