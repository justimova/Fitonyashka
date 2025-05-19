namespace Fytonyashka.Pages.Account;

using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LogoutModel : PageModel
{
    private readonly IAccountService _accountService;

    public LogoutModel(IAccountService accountService) {
        _accountService = accountService;
    }

    public IActionResult OnPost(){
        string? username = HttpContext.Session?.GetString("Username");
        _accountService.Logout(username);
        HttpContext.Session?.Remove("Username");
        return RedirectToPage("/Index");
    }
}