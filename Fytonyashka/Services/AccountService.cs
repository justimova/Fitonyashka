using Fytonyashka.DTOs;

namespace Fytonyashka.Services;

public interface IAccountService
{
    bool Login(string username, string password);
    void Logout(string username);
}

internal class AccountService : IAccountService
{
    private readonly IUserService _userService;
    private List<string> LoggedUserNames { get; set; } = new List<string>();

    public AccountService(IUserService userService) {
        _userService = userService;
    }

    public bool Login(string username, string password) {
        UserDto user = _userService.GetByUsername(username);
        if (user == null) {
            return false;
        }
        if (user.Password == password) {
            LoggedUserNames.Add(username);
            return true;
        }
        return false;
    }

    public void Logout(string username) {
        if (LoggedUserNames.Contains(username)) {
            LoggedUserNames.Remove(username);
        }
    }
}