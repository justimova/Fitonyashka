using Fytonyashka.Core;
using Fytonyashka.Services;
using Fytonyashka.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fytonyashka.Pages.Weight;

public class BmiModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IWeightService _weightService;
    public List<BmiRange> BmiRanges => Bmi.BmiRanges;

    public BmiModel(IUserService userService, IWeightService weightService) {
        _userService = userService;
        _weightService = weightService;
    }

    [BindProperty]
    public Bmi CurrentBmi { get; set; }

    [BindProperty]
    public Bmi CalculatedBmi { get; set; } = new Bmi();

    public IActionResult OnGet() {
        string username = HttpContext.Session.GetString("Username");
        var userDto = _userService.GetByUsername(username);
        if (userDto == null) {
            ModelState.AddModelError("", "User doesn't exist. Try later or text our support");
            return Page();
        }
        var lastWeight = _weightService.GetLastByUserId(userDto.Id);
        if(lastWeight != null) {
            CurrentBmi = new Bmi {
                Height = CalculatedBmi.Height = userDto.Height,
                Weight = CalculatedBmi.Weight = (double)lastWeight?.Weight
            };
        }

        return Page();
    }

    public string GetFormatBmiRange(double min, double max) =>
        max == double.MaxValue ? $"{min}+" : $"{min} - {max}";
}
