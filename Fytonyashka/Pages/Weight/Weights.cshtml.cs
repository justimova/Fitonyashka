using Fytonyashka.Pages.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages
{
    public class WeightModel : PageModel
    {
        private readonly IWeightService _weightService;

        [BindProperty]
        public List<WeightInputModel> Weights { get; set; } = new List<WeightInputModel>();

        public WeightModel(IWeightService weightService) {
            _weightService = weightService;
        }

        public void OnGet() {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            Weights = _weightService.GetAllByUserId(userId)
            .Select(w => new WeightInputModel {
               Id = w.Id,
               Date = w.Date,
               Weight = w.Weight
           }).ToList();
        }

        public IActionResult OnPostDelete(int id) {
           var result = _weightService.Delete(id);
           if (!result) {
               TempData["Error"] = "Failed to delete record";
           }
           return RedirectToPage();
        }
    }
}
