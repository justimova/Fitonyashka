using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fytonyashka.Services;
using Fytonyashka.DTOs;
using Fytonyashka.Pages.DataModels;

namespace Fytonyashka.Pages.User
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        public CreateModel(IUserService userService, IAccountService accountService)
        {
            _userService = userService;
            _accountService = accountService;
        }

        [BindProperty]
        public UserInputModel UserInput { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            UserDto userDto = new UserDto();
            userDto.UserName = UserInput.Username;
            userDto.Email = UserInput.Email;
            userDto.Password = UserInput.Password;

            var result = _userService.Create(userDto);
            _accountService.Create(userDto);

            if (result)
            {
                return RedirectToPage("/User/Users");
            }
            
            ModelState.AddModelError("", "Failed to create user");
            return Page();
        }
    }
}