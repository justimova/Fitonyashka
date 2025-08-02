using Fytonyashka.DataModels;
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
        public List<WeightInputModel> Weights { get; set; }

        [BindProperty]
        public WeightInputModel CurrentWeightEntry { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IWeightService weightService, IUserService userService)
        {
            _logger = logger;
            _weightService = weightService;
            _userService = userService;
        }

        public void OnGet(DateTime? date)
        {
            if (HttpContext.Session.GetString("Username") != null) {
                int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
                var userDto = _userService.GetById(userId);
                Weights = _weightService.GetAllByUserId(userId)
                    .Select(w => new WeightInputModel
                    {
                        Id = w.Id,
                        Date = w.Date,
                        Weight = w.Weight
                    }).OrderBy(e => e.Date)
                    .ToList();
                CurrentWeightEntry = date.HasValue ? Weights.LastOrDefault(w => w.Date == date.Value) : Weights.LastOrDefault();
            }
        }
        public string FormatDate(DateTime date) => date.Date switch {
            var d when d == DateTime.Today => "Today",
            var d when d == DateTime.Today.AddDays(-1) => "Yesterday",
            var d when d.Year == DateTime.Today.Year => d.ToString("ddd, MMM dd", System.Globalization.CultureInfo.InvariantCulture),
            _ => date.ToString("ddd, MMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture)
        };
    }
}
