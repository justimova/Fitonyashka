using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fytonyashka.DTOs;
using Fytonyashka.DataModels;
using Fytonyashka.Services.Interfaces;

namespace Fytonyashka.Pages.Weight
{
    public class UpdateModel : PageModel
    {
        private readonly IWeightService _weightService;

        public UpdateModel(IWeightService weightService) {
            _weightService = weightService;
        }

        [BindProperty]
        public WeightInputModel WeightInput { get; set; }

        public IActionResult OnGet(int weightId) {
            var weight = _weightService.GetById(weightId);
            if (weight == null) {
                return Page();
            }
            WeightInput = new WeightInputModel {
                Id = weight.Id,
                UserId = weight.UserId,
                Date = weight.Date,
                Weight = weight.Weight
            };
            return Page();
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            WeightDto weightDto = new WeightDto {
                Id = WeightInput.Id,
                UserId = WeightInput.UserId,
                Date = WeightInput.Date,
                Weight = WeightInput.Weight
            };
            var result = _weightService.Update(weightDto);

            if (result) {
                return RedirectToPage("/Weight/Weights");
            }
            
            ModelState.AddModelError("", "Failed to edit weight");
            return Page();
        } 
    }
}