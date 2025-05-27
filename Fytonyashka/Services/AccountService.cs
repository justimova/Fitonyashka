using System.Text.Json;
using Fytonyashka.DTOs;

namespace Fytonyashka.Services;

public interface IAccountService
{
    bool Login(string username, string password);
    void Logout(string username);
    AccountDto GetByUsername(string username);
    AccountDto Create(UserDto user);
    bool DeleteByUserId(int userId);
    bool Update(AccountDto accountDto);
}

internal class AccountService : IAccountService
{
    private int _nextId = 1;
    private readonly IUserService _userService;
    private readonly string _dataFilePath;
    private List<string> LoggedUserNames { get; set; } = new List<string>();
    private List<AccountDto> _accounts = new();


    public AccountService(IUserService userService) {
        _userService = userService;
        string baseDirectory = Directory.GetCurrentDirectory();
        string dataDirectory = Path.Combine(baseDirectory, "Data");
        if (!Directory.Exists(dataDirectory)) {
            Directory.CreateDirectory(dataDirectory);
        }
        _dataFilePath = Path.Combine(dataDirectory, "accounts.json");

        if (File.Exists(_dataFilePath)) {
            Load();
        }
    }

    private void Load() {
        string accountJsons = File.ReadAllText(_dataFilePath);
        _accounts = JsonSerializer.Deserialize<List<AccountDto>>(accountJsons) ?? new List<AccountDto>();
        InitializeNextId();
    }

    private void InitializeNextId() {
        foreach (AccountDto account in _accounts) {
            if (account.Id >= _nextId) {
                _nextId = account.Id + 1;
            }
        }
    }

    private void SaveToFile() {
        string accountsJson = JsonSerializer.Serialize(_accounts,
            new JsonSerializerOptions { WriteIndented = true }); // pretty print
        File.WriteAllText(_dataFilePath, accountsJson);
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

    public AccountDto GetByUsername(string username) {
        UserDto user = _userService.GetByUsername(username);
        if (user == null) {
            return null;
        }
        foreach (AccountDto account in _accounts) {
            if (account.UserId == user.Id) {
                if (account.User == null) {
                    account.User = user;
                }
                return account;
            }
        }
        return null;
    }

    public AccountDto Create(UserDto user) {
        AccountDto account = new AccountDto {
            Id = _nextId++,
            User = user,
            UserId = user.Id,
            FirstName = string.Empty,
            Birthday = DateTime.Now,
            Height = 0
        };
        _accounts.Add(account);
        SaveToFile();
        return account;
    }

    public bool DeleteByUserId(int userId) {
        foreach (AccountDto account in _accounts) {
            if (account.UserId == userId) {
                _accounts.Remove(account);
                SaveToFile();
                return true;
            }
        }
        return false;
    }

    public bool Update(AccountDto accountDto) {
        var account = GetById(accountDto.Id);
        if (account == null) {
            return false;
        }
        account.FirstName = accountDto.FirstName;
        account.Birthday = accountDto.Birthday;
        account.Height = accountDto.Height;
        SaveToFile();
        _userService.Update(accountDto.User);
        return true;
    }

    public AccountDto GetById(int id) {
        foreach (AccountDto account in _accounts) {
            if (account.Id == id) {
                account.User ??= _userService.GetById(account.UserId);
                return account;
            }
        }
        return null;
    }
}