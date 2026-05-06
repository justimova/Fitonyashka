using Fitonyashka.Services.Interfaces;
using Fitonyashka.ViewModels.Bmi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitonyashka.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class BmiController : ControllerBase
{
    private readonly IBmiService _bmiService;
    
    public BmiController(IBmiService bmiService) {
        _bmiService = bmiService;
    }

    [HttpGet]
    public ActionResult<CalculatedBmiViewModel> Calculate([FromQuery] int height, [FromQuery] decimal weight) {
        decimal bmi = _bmiService.CalculateBmi(height, weight);
        string category = _bmiService.GetBmiCategory(bmi);
        var viewModel = new CalculatedBmiViewModel(bmi, category);

        return Ok(viewModel);
    }

    [HttpGet]
    [Route("calculateWeight")]
    public ActionResult<decimal> CalculateWeight([FromQuery] int height, [FromQuery] decimal bmi) {
        decimal weight = _bmiService.CalculateWeight(height, bmi);

        return Ok(weight);
    }

    [HttpGet]
    [Route("categories")]
    public ActionResult<IReadOnlyCollection<BmiRangeViewModel>> GetCategories() {
        var bmiRanges = _bmiService.GetBmiCategories();
        var rangeViewModels = bmiRanges.Select(r => new BmiRangeViewModel(r.Min, r.Max, r.Category)).ToList();

        return Ok(rangeViewModels);
    }
}
