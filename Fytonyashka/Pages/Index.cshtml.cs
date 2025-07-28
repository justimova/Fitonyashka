using Fytonyashka.Pages.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWeightService _weightService;
        private readonly IUserService _userService;

        [BindProperty]
        public List<WeeklyWeightGroup> WeeklyWeights { get; set; }

        [BindProperty]
        public WeightInputModel LatestWeightEntry { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IWeightService weightService, IUserService userService)
        {
            _logger = logger;
            _weightService = weightService;
            _userService = userService;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("Username") != null) {
                int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
                var userDto = _userService.GetById(userId);
                var weights = _weightService.GetAllByUserId(userId)
                    .Select(w => new WeightInputModel
                    {
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
                    .Select(g => new WeeklyWeightGroup
                    {
                        WeekTitle = $"{g.Key.Start:dd MMMM} – {g.Key.End:dd MMMM yyyy}",
                        Entries = g.OrderByDescending(e => e.Date).ToList()
                    })
                    .ToList();
                LatestWeightEntry = WeeklyWeights
                    .SelectMany(w => w.Entries)
                    .OrderByDescending(e => e.Date)
                    .FirstOrDefault();
            }
        }
    }
}
