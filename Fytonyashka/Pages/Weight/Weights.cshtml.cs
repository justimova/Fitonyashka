using Fytonyashka.Core.DateRange;
using Fytonyashka.DTOs;
using Fytonyashka.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fytonyashka.Pages
{
    public class WeightModel : PageModel
    {
        private readonly IWeightService _weightService;
        private readonly IWeightDateRangeService _weightDateRangeService;
        private readonly IUserService _userService;

        [BindProperty]
        public List<WeeklyWeightGroup> WeeklyWeights { get; set; }

        [BindProperty]
        public List<WeightInputModel> GraphWeights { get; set; }

        public List<WeightInputModel> Weights { get; set; }

        [BindProperty]
        public List<SelectListItem> Periods { get; set; }

        [BindProperty]
        public string Period { get; set; }

        public WeightModel(IWeightService weightService,
                IWeightDateRangeService weightDateRangeService, IUserService userService) {
            _weightService = weightService;
            _weightDateRangeService = weightDateRangeService;
            _userService = userService;
        }

        public void OnGet() {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var userDto = _userService.GetById(userId);
            Weights = _weightService.GetAllByUserId(userId)
                .Select(w => new WeightInputModel {
                    Id = w.Id,
                    Date = w.Date,
                    Weight = w.Weight
                }).ToList();
            WeeklyWeights = Weights
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
            Periods = _weightDateRangeService.GetAll().Select(p => new SelectListItem {
                Value = p.Id.ToString(),
                Text = p.Text,
                Selected = userDto.SelectedDateRangeId == p.Id
            }).ToList();
            DateRangeDto selectedPeriod = _weightDateRangeService.GetById(userDto.SelectedDateRangeId);
            GraphWeights = Weights.OrderBy(w => w.Date).ToList();
            if (selectedPeriod.IsFiltered) {
                IDateRangeStrategy strategy = DateRangeStrategyFactory.GetStrategy((DateRangeOption)selectedPeriod.FilterDateRange);
                GraphWeights = GraphWeights.Where(w => strategy.IsInRange(w.Date, selectedPeriod.FilterNumber)).ToList();
            }
        }

        public IActionResult OnPostDelete(int id) {
           var result = _weightService.Delete(id);
           if (!result) {
               TempData["Error"] = "Failed to delete record";
           }
           return RedirectToPage();
        }

        public IActionResult OnPostChangePeriod() {
            var selectedPeriod = int.Parse(Period);
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            _userService.UpdateDateRange(userId, selectedPeriod);
            return RedirectToPage();
        }
    }
}
