using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Fytonyashka.Services;
using Fytonyashka.DTOs;

namespace Fytonyashka.Pages.User
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;

        public CreateModel(IUserService userService)
        {
            _userService = userService;
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

            if (result)
            {
                return RedirectToPage("/User/Users");
            }
            
            ModelState.AddModelError("", "Failed to create user");
            return Page();
        }
    }

    public class UserInputModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}