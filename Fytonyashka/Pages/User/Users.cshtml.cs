using Fytonyashka.Pages.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages
{
    public class UsersModel : PageModel
    {
        private readonly ILogger<UsersModel> _logger;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        [BindProperty]
        public List<UserInputModel> Users { get; set; } = new List<UserInputModel>();

        public UsersModel(ILogger<UsersModel> logger, IUserService userService, IAccountService accountService) {
            _logger = logger;
            _userService = userService; 
            _accountService = accountService;
        }

        public void OnGet() {
            Users = _userService.GetAll().Select(u => new UserInputModel {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email
            }).ToList();
        }

        public IActionResult OnPostDelete(int id) {
            var result = _userService.Delete(id);
            if (!result) {
                TempData["Error"] = "Failed to delete user";
            }
            _accountService.DeleteByUserId(id);
            return RedirectToPage();
        }
    }
}
