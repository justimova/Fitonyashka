using Fytonyashka.DTOs;
using Fytonyashka.DataModels;
using Fytonyashka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Goal
{
    public class GoalsModel : PageModel
    {
        private readonly IUserGoalService _userGoalService;
        private readonly IWeightService _weightService;

        public UserGoalModel CurrentGoal { get; set; }

        [BindProperty]
        public UserGoalModel InputGoal { get; set; } = new UserGoalModel();

        public GoalsModel(IUserGoalService userGoalService, IWeightService weightService) {
            _userGoalService = userGoalService;
            _weightService = weightService;
        }

        public IActionResult OnGet() {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var userGoal = _userGoalService.GetByUserId(userId);
            if (userGoal == null) {
                var lastWeight = _weightService.GetLastByUserId(userId);
                InputGoal = new UserGoalModel {
                    Weight = 0,
                    InitialWeight = lastWeight?.Weight ?? 0,
                    StartDate = lastWeight?.Date ?? DateTime.UtcNow.Date
                };
                return Page();
            }
            CurrentGoal = new UserGoalModel {
                Weight = userGoal.Weight,
                StartDate = userGoal.StartDate,
                InitialWeight = userGoal.InitialWeight
            };
            return Page();
        }

        public IActionResult OnPostSetGoal() {
            if (!ModelState.IsValid) {
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Validation failed";
                return Page();
            }

            try {
                int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
                var result = _userGoalService.Create(new UserGoalDto() {
                    UserId = userId,
                    StartDate = InputGoal.StartDate,
                    Weight = InputGoal.Weight,
                    InitialWeight = InputGoal.InitialWeight
                });
                if (result.IsSuccess) {
                    TempData["ToastTitle"] = "Success";
                    TempData["ToastMessage"] = $"Goal {InputGoal.Weight} kg saved successfully!";
                    return RedirectToPage();
                }
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Error saving goal: " + result.ErrorMessage;
                return Page();
            } catch (Exception ex) {
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Error saving goal: " + ex.Message;
                return Page();
            }
        }

        public IActionResult OnPostDelete() {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var result = _userGoalService.Delete(userId);
            if (result.IsSuccess) {
                TempData["ToastTitle"] = "Success";
                TempData["ToastMessage"] = $"Goal deleted successfully!";
                return RedirectToPage();
            }
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Error deleting goal: " + result.ErrorMessage;
            return Page();
        }

        public string GetGoal() => CurrentGoal.Weight < CurrentGoal.InitialWeight ? "Lose weight" : "Weight gain";
    }
}
