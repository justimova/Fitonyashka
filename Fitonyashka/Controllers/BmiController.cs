using Fitonyashka.ViewModels.Bmi;
using Fytonyashka.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitonyashka.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class BmiController : ControllerBase
{
    [HttpGet]
    public ActionResult<CalculatedBmiViewModel> Calculate([FromQuery] int height, [FromQuery] decimal weight) {
        var bmi = new Bmi { Height = height, Weight = weight };
        return Ok(new CalculatedBmiViewModel(bmi.BmiValue, bmi.BmiCategory));
    }

    [HttpGet]
    [Route("categories")]
    public ActionResult<IReadOnlyCollection<BmiRangeViewModel>> GetCategory() {
        var rangeViewModels = Bmi.BmiRanges.Select(r => new BmiRangeViewModel (r.Min, r.Max, r.Category)).ToList();
        return Ok(rangeViewModels);
    }
}
