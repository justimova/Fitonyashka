using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fytonyashka.Services;
using Fytonyashka.DTOs;
using Fytonyashka.DataModels;

namespace Fytonyashka.Pages.User
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;

        public CreateModel(IUserService userService) {
            _userService = userService;
        }

        [BindProperty]
        public UserInputModel UserInput { get; set; }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }
            UserDto userDto = new UserDto();
            userDto.UserName = UserInput.Username;
            userDto.Email = UserInput.Email;
            userDto.Password = UserInput.Password;

            var result = _userService.Create(userDto);

            if (result.IsSuccess) {
                return RedirectToPage("/User/Users");
            }
            
            ModelState.AddModelError("", "Failed to create user");
            return Page();
        }
    }
}