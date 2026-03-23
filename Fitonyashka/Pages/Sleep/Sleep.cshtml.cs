using Fytonyashka.DataModels;
using Fytonyashka.DTOs;
using Fytonyashka.Services;
using Fytonyashka.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Sleep;

public class SleepModel : PageModel
{
    private readonly ISleepService _sleepService;

    [BindProperty]
    public List<SleepInfoModel> DailySleeps { get; set; }

    [BindProperty]
    public SleepInputModel SleepInputModel { get; set; }

    public SleepModel(ISleepService sleepService)
    {
        _sleepService = sleepService;
    }

    public IActionResult OnGet() {
        int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        SleepInputModel = new SleepInputModel {
            DateFrom = DateOnly.FromDateTime(DateTime.Now),
            DateTo = DateOnly.FromDateTime(DateTime.Now),
            UserId = userId
        };
        DailySleeps = _sleepService.GetAllByUserId(userId)
            .Select(s => new SleepInfoModel {
                Id = s.Id,
                Date = s.DateFrom,
                SleepDuration = GetFormattedDuration(s.DateTo - s.DateFrom)
            }).ToList();
        return Page();
    }

    public IActionResult OnPostEnterSleep() {
        if (!ModelState.IsValid) {
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Validation failed";
            return Page();
        }

        try {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var result = _sleepService.Create(new SleepDto() {
                UserId = userId,
                DateFrom = SleepInputModel.DateFrom.ToDateTime(SleepInputModel.TimeFrom),
                DateTo = SleepInputModel.DateTo.ToDateTime(SleepInputModel.TimeTo)
            });
            if (result.IsSuccess) {
                TempData["ToastTitle"] = "Success";
                TempData["ToastMessage"] = $"Date of sleep saved successfully!";
                return RedirectToPage();
            }
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Error saving date of sleep: " + result.ErrorMessage;
            return Page();
        } catch (Exception ex) {
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Error saving date of sleep: " + ex.Message;
            return Page();
        }
    }

    public IActionResult OnPostDelete(int id) {
        var result = _sleepService.Delete(id);
        if (!result.IsSuccess) {
            TempData["Error"] = result.ErrorMessage;
        }
        return RedirectToPage();
    }

    private string GetFormattedDuration(TimeSpan duration) =>
        $"{GetDurationPart(duration.Days, "d")}{GetDurationPart(duration.Hours, "h")}{GetDurationPart(duration.Minutes, "m")}".Trim();

    private string GetDurationPart(int duration, string durationName) => duration != 0 ? $"{duration} {durationName} " : "";
}
