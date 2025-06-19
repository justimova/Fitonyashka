namespace Fytonyashka.Pages.Account;

using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LogoutModel : PageModel
{
    private readonly IUserService _userService;

    public LogoutModel(IUserService userService) {
        _userService = userService;
    }

    public IActionResult OnPost(){
        string? username = HttpContext.Session?.GetString("Username");
        _userService.Logout(username);
        HttpContext.Session?.Remove("Username");
        HttpContext.Session?.Remove("UserId");
        HttpContext.Session?.Remove("AvatarFileName");
        return RedirectToPage("/Index");
    }
}