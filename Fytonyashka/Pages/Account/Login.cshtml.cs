using Fytonyashka.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        [BindProperty]
        public LoginInputModel LoginInput { get; set; } = new LoginInputModel();

        public LoginModel(IAccountService accountService) {
            _accountService = accountService;
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (_accountService.Login(LoginInput.UserName, LoginInput.Password))
            {
                HttpContext.Session.SetString("Username", LoginInput.UserName);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return Page();
        }
    }
}