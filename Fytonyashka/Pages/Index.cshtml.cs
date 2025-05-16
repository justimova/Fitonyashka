using Fytonyashka.Pages.User;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserService _userService;

        [BindProperty]
        public List<UserInputModel> Users { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public void OnGet()
        {
            Users = _userService.GetAll().Select(u => new UserInputModel {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email
            }).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            var result = _userService.Delete(id);
            if (!result)
            {
                TempData["Error"] = "Failed to delete user";
            }
            return RedirectToPage();
        }
    }
}
