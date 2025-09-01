using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fytonyashka.DTOs;
using Fytonyashka.DataModels;
using Fytonyashka.Services.Interfaces;

namespace Fytonyashka.Pages.Weight
{
    public class CreateModel : PageModel
    {
        private readonly IWeightService _weightService;

        public CreateModel(IWeightService weightService) {
            _weightService = weightService;
        }

        [BindProperty]
        public WeightInputModel WeightInput { get; set; }

        [BindProperty]
        public bool OverwriteConfirmed { get; set; }

        public IActionResult OnGet(int userId) { 
            WeightInput = new WeightInputModel();
            WeightInput.UserId = userId;
            return Page();
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            var isExists = _weightService
                .GetAllByUserId(WeightInput.UserId)
                .Any(w => w.Date == WeightInput.Date);

            if (isExists && !OverwriteConfirmed) {
                TempData["ShowConfirmation"] = true;
                return Page();
            }

            WeightDto weightDto = new WeightDto {
                Id = WeightInput.Id,
                UserId = WeightInput.UserId,
                Date = WeightInput.Date,
                Weight = WeightInput.Weight
            };
            var result = _weightService.Create(weightDto);

            if (result) {
                return RedirectToPage("/Weight/Weights");
            }
            
            ModelState.AddModelError("", "Failed to enter weight");
            return Page();
        } 
    }
}