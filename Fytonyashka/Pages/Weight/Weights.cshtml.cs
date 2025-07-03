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
        public List<WeeklyWeightGroup> WeeklyWeights { get; set; }

        public WeightModel(IWeightService weightService) {
            _weightService = weightService;
        }

        public void OnGet() {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            List<WeightInputModel> weights = _weightService.GetAllByUserId(userId)
                .Select(w => new WeightInputModel {
                    Id = w.Id,
                    Date = w.Date,
                    Weight = w.Weight
                }).ToList();

            WeeklyWeights = weights
                .GroupBy(w => {
                    var monday = w.Date.Date.AddDays(-(int)w.Date.DayOfWeek + (w.Date.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
                    var sunday = monday.AddDays(6);
                    return new { Start = monday, End = sunday };
                })
                .OrderByDescending(g => g.Key.Start)
                .Select(g => new WeeklyWeightGroup {
                    WeekTitle = $"{g.Key.Start:dd MMMM} – {g.Key.End:dd MMMM yyyy}",
                    Entries = g.OrderByDescending(e => e.Date).ToList()
                })
                .ToList();
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
