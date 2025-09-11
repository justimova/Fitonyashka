using Fytonyashka.DataModels;
using Fytonyashka.Services.Interfaces;
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
                ModelState.AddModelError("", "User doesn't exist. Try later or text our support");
                return Page();
            }
            ChangePasswordInput.Id = userDto.Id;
            return Page();
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }
            if (!_userService.CheckPassword(ChangePasswordInput.Id, ChangePasswordInput.Password)) {
                ModelState.AddModelError("ChangePasswordInput.Password", "Current password is incorrect");
                return Page();
            }
            _userService.ChangePassword(ChangePasswordInput.Id, ChangePasswordInput.NewPassword);
            return RedirectToPage();
        }

        public IActionResult OnPostDelete() {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var result = _userService.Delete(userId);
            if (!result.IsSuccess) {
                TempData["Error"] = "Failed to delete user";
                return Page();
            }
            string? username = HttpContext.Session?.GetString("Username");
            HttpContext.Session?.Remove("Username");
            HttpContext.Session?.Remove("UserId");
            HttpContext.Session?.Remove("AvatarFileName");
            return RedirectToPage("/Index");
        }
    }
}
