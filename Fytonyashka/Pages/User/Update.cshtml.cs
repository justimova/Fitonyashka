using Fytonyashka.DTOs;
using Fytonyashka.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.User
{
    public class UpdateModel : PageModel
    {
        private readonly IUserService _userService;
        [BindProperty]
        public UserInputModel UserInput { get; set; } = new UserInputModel();

        public UpdateModel(IUserService userService) {
            _userService = userService;
        }
        
        public IActionResult OnGet(int id) { 
            var userDto = _userService.GetById(id);
            if (userDto == null) {
                ModelState.AddModelError("", "User doesn't exist. Try later or text our support");
                return Page();
            }
            UserInput = new UserInputModel {
                Id = userDto.Id,
                Username = userDto.UserName,
                Email = userDto.Email,
            };
            return Page();
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }
            _userService.Update(new UserDto { // TODO: #2
                Id = UserInput.Id,
                UserName = UserInput.Username,
                Email = UserInput.Email,
                Password = UserInput.Password
            });
            return RedirectToPage("/User/Users");
        }
    }
}
