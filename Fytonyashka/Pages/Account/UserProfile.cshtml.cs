using Fytonyashka.DTOs;
using Fytonyashka.Pages.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Account
{
    public class UserProfileModel : PageModel
    {
        private readonly IAccountService _accountService;
        [BindProperty]
        public AccountInputModel AccountInput { get; set; } = new AccountInputModel();

        public UserProfileModel(IAccountService accountService) {
            _accountService = accountService;
        }
        
        public IActionResult OnGet() { 
            string username = HttpContext.Session.GetString("Username");
            var accountDto = _accountService.GetByUsername(username);
            if (accountDto == null) {
                return NotFound(); // TODO: #2
            }
            AccountInput = new AccountInputModel {
                Id = accountDto.Id,
                User = new UserInputModel {
                    Id = accountDto.User.Id,
                    Username = accountDto.User.UserName,
                    Email = accountDto.User.Email,
                    Password = accountDto.User.Password
                },
                Birthday = accountDto.Birthday,
                FirstName = accountDto.FirstName,
                Height = accountDto.Height
            };
            return Page();
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }
            _accountService.Update(new AccountDto { // TODO: #2
                Id = AccountInput.Id,
                User = new UserDto {
                    Id = AccountInput.User.Id,
                    UserName = AccountInput.User.Username,
                    Email = AccountInput.User.Email,
                },
                UserId = AccountInput.User.Id,
                FirstName = AccountInput.FirstName,
                Birthday = AccountInput.Birthday,
                Height = AccountInput.Height
            });
            return RedirectToPage("/Account/UserProfile");
        }
    }
}