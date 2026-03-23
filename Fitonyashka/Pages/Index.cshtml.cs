using Fytonyashka.DataModels;
using Fytonyashka.Services;
using Fytonyashka.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWeightService _weightService;
        private readonly IUserService _userService;
        private readonly IUserGoalService _userGoalService;

        public UserGoalModel CurrentGoal { get; set; }

        public string GoalMessage { get; set; }

        [BindProperty]
        public List<WeightInputModel> Weights { get; set; }

        [BindProperty]
        public WeightInputModel CurrentWeightEntry { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IWeightService weightService, IUserService userService, IUserGoalService userGoalService)
        {
            _logger = logger;
            _weightService = weightService;
            _userService = userService;
            _userGoalService = userGoalService;
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
                var userGoal = _userGoalService.GetByUserId(userId);
                if (userGoal != null) {
                    CurrentGoal = new UserGoalModel {
                        Weight = userGoal.Weight,
                        StartDate = userGoal.StartDate,
                        InitialWeight = userGoal.InitialWeight
                    };
                    var lastWeight = _weightService.GetLastByUserId(userId);
                    if (lastWeight != null) {
                        if (CurrentGoal.InitialWeight >= CurrentGoal.Weight) {
                            if (lastWeight.Weight > CurrentGoal.Weight)
                                GoalMessage = $"Keep going, you’re closer every day!<br>{Math.Abs(lastWeight.Weight - CurrentGoal.Weight)} kg left";
                            else if (lastWeight.Weight == CurrentGoal.Weight)
                                GoalMessage = "Congrats! You’ve reached your goal!";
                            else
                                GoalMessage = $"Amazing! You went beyond your goal!<br>{Math.Abs(lastWeight.Weight - CurrentGoal.Weight)} kg over";
                        } else {
                            if (lastWeight.Weight < CurrentGoal.Weight)
                                GoalMessage = $"Keep working, you’re getting stronger!<br>{Math.Abs(lastWeight.Weight - CurrentGoal.Weight)} kg left";
                            else if (lastWeight.Weight == CurrentGoal.Weight)
                                GoalMessage = "Keep working, you’re getting stronger!";
                            else
                                GoalMessage = $"Fantastic! You’ve exceeded your goal!<br>{Math.Abs(lastWeight.Weight - CurrentGoal.Weight)} kg over";
                        }
                    }
                }
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
